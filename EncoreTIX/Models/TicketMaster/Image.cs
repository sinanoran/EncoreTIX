using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class Image
{
	[JsonPropertyName("url")]
	public string? Url { get; set; }
}
