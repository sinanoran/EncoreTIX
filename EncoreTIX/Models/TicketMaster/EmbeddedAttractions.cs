using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class EmbeddedAttractions
{
	[JsonPropertyName("attractions")]
	public List<Attraction>? Attractions { get; set; }
}
