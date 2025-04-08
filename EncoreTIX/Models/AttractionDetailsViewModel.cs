namespace EncoreTIX.Models;

public class AttractionDetailsViewModel
{
	public string Id { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
	public Dictionary<string, string> ExternalLinks { get; set; } = new();
	public List<EventViewModel> Events { get; set; } = new();
}
