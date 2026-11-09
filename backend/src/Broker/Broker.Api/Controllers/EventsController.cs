using Microsoft.AspNetCore.Mvc;

namespace Broker.Api.Controllers;

[ApiController]
[Route("topics/{topic}/events")]
public sealed class EventsController(InMemoryTopicStore store) : ControllerBase
{
    [HttpPost]
    public ActionResult<EventEnvelope> Publish(string topic, [FromBody] PublishEventRequest request)
    {
        var envelope = store.Append(topic, request.EventType, request.PayloadJson, request.OccurredAtUtc);
        return CreatedAtAction(nameof(Publish), new { topic }, envelope);
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<EventEnvelope>> GetAfter(string topic, [FromQuery] long after = 0, [FromQuery] string? group = null)
    {
        // `group` is accepted for symmetry with a real consumer-group API but isn't used to
        // track server-side offsets here — each consumer keeps its own cursor (see plan).
        return Ok(store.GetAfter(topic, after));
    }
}
