using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class EventDates
{
	[JsonPropertyName("start")]
	public EventStart? Start { get; set; }
}
