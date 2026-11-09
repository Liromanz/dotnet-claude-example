namespace MetricBoard.Common.Application;

/// <summary>Thrown by a command/query handler when the requested aggregate doesn't exist.</summary>
public sealed class NotFoundException(string message) : Exception(message);
