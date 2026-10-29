using System.Text.Json;

using Dapper;

using Lexicora.Api.Extensions;
using Lexicora.Api.Infrastructure.Database;
using Lexicora.Api.Infrastructure.Json.Contexts;
using Lexicora.Api.Infrastructure.RouteConstraints;
using Lexicora.Persistence.Postgres;

using Microsoft.EntityFrameworkCore;

using Npgsql;

using Scalar.AspNetCore;

// Tells Dapper.AOT to generate interceptors for the API assembly
[module: DapperAot]

namespace Lexicora.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Ulid.DefaultGenerationOptions = new Ulid.GenerationOptions
        {
            Monotonicity = Ulid.GenerationOptions.MonotonicityOptions.NonMonotonic
            // Important for slug time parts (ulidString[..12]) time + 2 random chars.
            // Maybe switch to 4 byte monotonic, if using the last 10 or 12 characters for slugs.

            // Both below are the default RandomSources, so no need to manually set them.
            //InitialRandomSource = new CryptographicallySecureRandomProvider(),
            //IncrementRandomSource = new PseudoRandomProvider()
        };

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Add validation
        builder.Services.AddValidation();

        // Register source generated JSON contexts
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.AllowTrailingCommas = true;

            // Reflection-based serialization is disabled (see Directory.Build.props),
            // so every request/response type must come from one of these source-generated contexts.
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default);
            options.SerializerOptions.TypeInfoResolverChain.Insert(1, TopicsJsonContext.Default);
            options.SerializerOptions.TypeInfoResolverChain.Insert(2, UsersJsonContext.Default);
            options.SerializerOptions.TypeInfoResolverChain.Insert(3, EntriesJsonContext.Default);
        });

        builder.Services.Configure<RouteOptions>(options =>
        {
            options.ConstraintMap.Add("email", typeof(EmailConstraint));
            options.ConstraintMap.Add("ulid", typeof(UlidConstraint));
            options.ConstraintMap.Add("username", typeof(UsernameConstraint));
            options.ConstraintMap.Add("slug", typeof(SlugConstraint));
        });

        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.Remove("traceId");

                // Check if the error is specifically a bad JSON request
                if (context.Exception is BadHttpRequestException badRequestEx)
                {
                    context.ProblemDetails.Title = "Invalid Request Content";
                    context.ProblemDetails.Status = StatusCodes.Status400BadRequest;
                    context.ProblemDetails.Detail = "The provided JSON is malformed and could not be parsed.";

                    // Optional: In Development, show the specific JSON error (like the trailing comma)
                    // In Production, keep it vague for security.
#if DEBUG
                    if (badRequestEx.InnerException is JsonException jsonEx)
                    {
                        context.ProblemDetails.Extensions["jsonError"] = jsonEx.Message;
                    }
#endif
                }
            };
        });

        // Zero-setup mode: run a throwaway PostgreSQL server alongside the API instead of connecting to an existing one.
        await using var embeddedPostgres = builder.Configuration.GetValue<bool>("Database:Embedded")
            ? await EmbeddedPostgres.StartAsync()
            : null;

        // Configure the database connection and register the data source as a singleton
        var connectionString = embeddedPostgres?.ConnectionString
            ?? builder.Configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException(
                "Connection string 'Postgres' is not configured. Set 'ConnectionStrings:Postgres' via appsettings, user secrets or the 'ConnectionStrings__Postgres' environment variable.");

        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();

        builder.Services.AddSingleton(dataSource);

        // Register the DbContext with the same data source
        builder.Services.AddDbContextPool<AppDbContext>(options =>
        {
            // Tell EF Core to use the exact same Data Source Dapper is using!
            options.UseNpgsql(dataSource);

            // Good for local debugging, remove in production:
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        }/*, poolSize: 1024 default size*/);

        var app = builder.Build();

        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            // Client errors like malformed JSON surface as BadHttpRequestException, keep their 4xx status instead of a 500.
            StatusCodeSelector = exception => exception is BadHttpRequestException badRequestEx
                ? badRequestEx.StatusCode
                : StatusCodes.Status500InternalServerError,

            // A client sending a bad request is not a server error, so don't log it as one.
            SuppressDiagnosticsCallback = context => context.Exception is BadHttpRequestException
        });

        // Apply pending migrations, so a fresh local database works with a plain `dotnet run`.
        if (app.Environment.IsDevelopment() || embeddedPostgres is not null)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync();

            // The embedded database starts empty every time, so give it something to show.
            if (embeddedPostgres is not null) await DemoData.SeedAsync(dbContext);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .WithTheme(ScalarTheme.Purple)
                    .HideClientButton()
                    .WithTitle("Lexicora API Reference")
                    .ShowOperationId()
                    .SortOperationsByMethod();
            });
        }

        app.UseStatusCodePages();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // TODO: Maybe add versioning here in the future?

        app.MapConfiguredEndpoints();

        await app.RunAsync();

        // TODO: Add cors configuration if needed, maybe with a policy that can be applied to specific endpoints.
        // TODO: Potentially add rate limiting, health checks etc.
    }
}
