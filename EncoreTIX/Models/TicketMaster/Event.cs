using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class Event
{
	[JsonPropertyName("id")]
	public string? Id { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("dates")]
	public EventDates? Dates { get; set; }

	[JsonPropertyName("images")]
	public List<Image>? Images { get; set; }

	[JsonPropertyName("_embedded")]
	public EmbeddedVenues? Embedded { get; set; }
}
