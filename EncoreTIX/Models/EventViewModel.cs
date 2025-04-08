namespace EncoreTIX.Models;

public class EventViewModel
{
	public string? Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Venue { get; set; } = string.Empty;
	public string Date { get; set; } = string.Empty;
	public string? ImageUrl  { get; set; }
}
