using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class Attraction
{
	[JsonPropertyName("id")]
	public string? Id { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("images")]
	public List<Image>? Images { get; set; }

	[JsonPropertyName("externalLinks")]
	public Dictionary<string, List<Link>>? ExternalLinks { get; set; }
}
