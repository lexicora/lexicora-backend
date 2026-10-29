using System.Diagnostics;

using MysticMind.PostgresEmbed;

using Npgsql;

namespace Lexicora.Api.Infrastructure.Database;

/// <summary>
/// A throwaway PostgreSQL server, which runs alongside the API, so it can be tried out without installing a database.
/// </summary>
/// <remarks>
/// Enabled with <c>Database:Embedded=true</c> (see the <c>demo</c> launch profile). This is a real PostgreSQL server,
/// not an in-memory substitute, so the Postgres specific SQL, <c>citext</c> columns and check constraints behave exactly as in production.
/// The binaries are downloaded once into the build output folder. The data is deleted when the API shuts down.
/// </remarks>
public sealed class EmbeddedPostgres : IAsyncDisposable
{
    private const string PostgresVersion = "17.11.0";

    private readonly PgServer _server;

    private EmbeddedPostgres(PgServer server) => _server = server;

    public string ConnectionString => new NpgsqlConnectionStringBuilder
    {
        Host = "localhost",
        Port = _server.PgPort,
        Username = "postgres",
        Database = "lexicora_db"
    }.ConnectionString;

    public static async Task<EmbeddedPostgres> StartAsync()
    {
        var baseDir = GetBaseDirectory();
        var embedDir = Path.Combine(baseDir, "pg_embed"); // PostgresEmbed appends "pg_embed" to the given directory.

        StopOrphanedInstances(embedDir);

        Console.WriteLine($"Starting embedded PostgreSQL {PostgresVersion} (the first run downloads it into '{embedDir}')...");

        var server = new PgServer(PostgresVersion, dbDir: baseDir, clearInstanceDirOnStop: true);
        await server.StartAsync();

        return new EmbeddedPostgres(server);
    }

    public async ValueTask DisposeAsync() => await _server.DisposeAsync();

    /// <summary>
    /// Keeps the binaries next to the build output, so everything stays inside the (git ignored) bin folder of the repository.
    /// </summary>
    private static string GetBaseDirectory()
    {
        var baseDir = AppContext.BaseDirectory;

        // PostgresEmbed passes the path unquoted to chmod, which breaks on paths with spaces. Use the temp folder instead.
        if (!OperatingSystem.IsWindows() && baseDir.Contains(' '))
        {
            Console.WriteLine("The build output path contains spaces, which embedded PostgreSQL doesn't support. Using the temp folder instead.");
            return Path.Combine(Path.GetTempPath(), "lexicora");
        }

        return baseDir;
    }

    /// <summary>
    /// Stops and removes servers left behind by a previous run, which was killed before it could shut down gracefully.
    /// </summary>
    /// <remarks>
    /// The server runs as a daemon (via pg_ctl), not as a child process of the API, so it outlives a killed API process.
    /// Only one API instance can run at a time (the port is fixed), so any existing instance is a leftover.
    /// </remarks>
    private static void StopOrphanedInstances(string embedDir)
    {
        if (!Directory.Exists(embedDir)) return;

        foreach (var instanceDir in Directory.EnumerateDirectories(embedDir))
        {
            // Instances are stored in directories named by a Guid, next to the shared "binaries" directory.
            if (!Guid.TryParse(Path.GetFileName(instanceDir), out _)) continue;

            var pgCtl = Path.Combine(instanceDir, "bin", OperatingSystem.IsWindows() ? "pg_ctl.exe" : "pg_ctl");
            var dataDir = Path.Combine(instanceDir, "data");

            if (File.Exists(pgCtl) && File.Exists(Path.Combine(dataDir, "postmaster.pid")))
            {
                using var process = Process.Start(new ProcessStartInfo(pgCtl)
                {
                    ArgumentList = { "stop", "-D", dataDir, "-m", "immediate" },
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });
                process?.WaitForExit(TimeSpan.FromSeconds(10));
            }

            try
            {
                Directory.Delete(instanceDir, recursive: true);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                Console.WriteLine($"Could not remove the leftover embedded PostgreSQL instance '{instanceDir}': {ex.Message}");
            }
        }
    }
}
