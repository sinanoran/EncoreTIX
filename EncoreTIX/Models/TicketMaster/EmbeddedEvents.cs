using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class EmbeddedEvents
{
	[JsonPropertyName("events")]
	public List<Event>? Events { get; set; }
}
