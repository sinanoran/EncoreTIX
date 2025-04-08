using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class EmbeddedVenues
{
	[JsonPropertyName("venues")]
	public List<Venue>? Venues { get; set; }
}
