using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class EventStart
{
	[JsonPropertyName("localDate")]
	public string? LocalDate { get; set; }
}
