using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class EventRoot
{
	[JsonPropertyName("_embedded")]
	public EmbeddedEvents? Embedded { get; set; }
}
