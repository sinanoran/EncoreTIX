using System.Text.Json.Serialization;

namespace EncoreTIX.Models.TicketMaster;

public class AttractionRoot
{
	[JsonPropertyName("_embedded")]
	public EmbeddedAttractions? Embedded { get; set; }
}
