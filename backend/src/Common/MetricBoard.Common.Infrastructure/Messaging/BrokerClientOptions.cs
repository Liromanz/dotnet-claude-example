namespace MetricBoard.Common.Infrastructure.Messaging;

public sealed class BrokerClientOptions
{
    public const string SectionName = "Broker";

    /// <summary>Base URL of Broker.Api, e.g. http://localhost:5080</summary>
    public required string BaseUrl { get; set; }

    /// <summary>How often a consumer polls the topic for new events.</summary>
    public int PollingIntervalMs { get; set; } = 1000;
}
