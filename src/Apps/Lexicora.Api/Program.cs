using Dapper;

using Lexicora.Api.Extensions;
using Lexicora.Api.Infrastructure.Json.Contexts;
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

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Add validation
        builder.Services.AddValidation();

        // Register source generated JSON contexts
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
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

        // Maybe add later if needed manual mapping might be slightly slower, but is clearer.
        //SqlMapper.AddTypeHandler(new UlidTypeHandler());

        // Configure the database connection and register the data source as a singleton
        var connectionString = builder.Configuration.GetConnectionString("Postgres");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

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
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // TODO: Maybe add versioning here in the future?

        app.MapConfiguredEndpoints();

        app.Run();

        // TODO: Add cors configuration if needed, maybe with a policy that can be applied to specific endpoints.
        // TODO: Potentially add global error handling middleware, logging, etc.
    }
}
