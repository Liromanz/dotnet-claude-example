using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MetricBoard.Common.Infrastructure.Persistence;

/// <summary>
/// SQLite's EF Core provider can't translate ORDER BY / comparisons on a raw DateTimeOffset
/// column. Storing it as a round-trip ("O") UTC string keeps it both human-readable in the
/// .db file and lexicographically sortable, so ORDER BY on the column works as plain text
/// comparison — every service's DateTimeOffset columns should use this converter.
/// </summary>
public sealed class SortableDateTimeOffsetConverter()
    : ValueConverter<DateTimeOffset, string>(
        v => v.UtcDateTime.ToString("O", CultureInfo.InvariantCulture),
        v => DateTimeOffset.Parse(v, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal));
