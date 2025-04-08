using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class Link
{
	[JsonPropertyName("url")]
	public string? Url { get; set; }
}
