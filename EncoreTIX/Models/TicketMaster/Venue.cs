using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class Venue
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }
}
