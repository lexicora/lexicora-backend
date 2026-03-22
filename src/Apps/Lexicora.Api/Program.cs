using System.Text.Json;

using Dapper;

using Lexicora.Api.Extensions;
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
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Ulid.DefaultGenerationOptions = new Ulid.GenerationOptions
        {
            Monotonicity = Ulid.GenerationOptions.MonotonicityOptions.MonotonicRandom4Byte,
            InitialRandomSource = new CryptographicallySecureRandomProvider(),
            IncrementRandomSource = new PseudoRandomProvider()
        };

        //SqlMapper.AddTypeHandler(new JsonElementTypeHandler());

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
            //options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Disallow; // already default behavior
            //options.SerializerOptions.MaxDepth = 32; // default is 64, adjust if necessary

            //var combinedResolver = JsonTypeInfoResolver.Combine(
            //    AppJsonContext.Default
            //    WeatherForecastJsonContext.Default
            //);

            //options.SerializerOptions.TypeInfoResolverChain.Insert(0, combinedResolver);
            // Alternatively, you can set the TypeInfoResolver directly if you don't have other resolvers to combine.

            // Maybe implement source generator that iterates through all contexts and lists them here.
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default);
            options.SerializerOptions.TypeInfoResolverChain.Insert(1, TopicsJsonContext.Default);
            options.SerializerOptions.TypeInfoResolverChain.Insert(2, UsersJsonContext.Default);
        });

        builder.Services.Configure<RouteOptions>(options =>
        {
            options.ConstraintMap.Add("email", typeof(EmailConstraint));
            options.ConstraintMap.Add("ulid", typeof(UlidConstraint));
            options.ConstraintMap.Add("username", typeof(UsernameConstraint));
            //options.ConstraintMap.Add("slug", typeof(SlugConstraint));
        });

        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.Remove("traceId");
                //context.ProblemDetails.Extensions.Remove("requestId");

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

        // Maybe add later if needed manual mapping might be slightly slower, but is clearer.
        //SqlMapper.AddTypeHandler(new UlidTypeHandler());
        //SqlMapper.AddTypeHandler(new JsonElementTypeHandler());

        // Configure the database connection and register the data source as a singleton
        var connectionString = builder.Configuration.GetConnectionString("Postgres");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        // Add Json configuration here so that Dapper and EF Core for that part can properly work with jsonb columns.
        //var jsonOptions = new JsonSerializerOptions
        //{
        //    TypeInfoResolver = AppJsonContext.Default
        //};
        //dataSourceBuilder.ConfigureJsonOptions(jsonOptions);
        //dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

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

        app.UseExceptionHandler();
        //app.UseExceptionHandler(exceptionHandlerApp =>
        //{
        //    exceptionHandlerApp.Run(async httpContext =>
        //    {
        //        var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
        //        if (pds == null
        //            || !await pds.TryWriteAsync(new ProblemDetailsContext { HttpContext = httpContext }))
        //        {
        //            // Fallback behavior
        //            await httpContext.Response.WriteAsync("Fallback: An error occurred.");
        //        }
        //    });
        //});

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .WithTheme(ScalarTheme.Purple)
                    //.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)
                    .HideClientButton()
                    .WithTitle("Lexicora API Reference");
            });
            app.UseDeveloperExceptionPage();
        }

        app.UseStatusCodePages();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // TODO: Maybe add versioning here in the future?

        app.MapConfiguredEndpoints();

        app.Run();

        // TODO: Add cors configuration if needed, maybe with a policy that can be applied to specific endpoints.
        // TODO: Potentially add global error handling middleware, logging, rate limiting etc.
    }
}
