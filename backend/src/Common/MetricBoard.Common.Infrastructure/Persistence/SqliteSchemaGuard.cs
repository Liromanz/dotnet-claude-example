using System.Data;
using Microsoft.EntityFrameworkCore;

namespace MetricBoard.Common.Infrastructure.Persistence;

/// <summary>
/// EF Core migrations are disabled for this project — Liquibase owns the schema. This guard
/// runs once at startup so a missing `liquibase update` fails fast with a clear message
/// instead of surfacing as a confusing "no such table" error on the first request.
/// </summary>
public static class SqliteSchemaGuard
{
    public static async Task EnsureTablesExistAsync(
        DbContext dbContext,
        IReadOnlyCollection<string> expectedTables,
        CancellationToken cancellationToken = default)
    {
        var connection = dbContext.Database.GetDbConnection();
        var wasClosed = connection.State != ConnectionState.Open;
        if (wasClosed)
        {
            await connection.OpenAsync(cancellationToken);
        }

        try
        {
            var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            await using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table'";
                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    existing.Add(reader.GetString(0));
                }
            }

            var missing = expectedTables.Where(table => !existing.Contains(table)).ToList();
            if (missing.Count > 0)
            {
                throw new InvalidOperationException(
                    $"Database is missing table(s): {string.Join(", ", missing)}. " +
                    "EF Core migrations are disabled on purpose — run `liquibase update` " +
                    "against this service's changelog before starting it.");
            }
        }
        finally
        {
            if (wasClosed)
            {
                await connection.CloseAsync();
            }
        }
    }
}
