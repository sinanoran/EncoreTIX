using EncoreTIX.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TicketMaster = EncoreTIX.Models.TicketMaster;

namespace EncoreTIX.Services;

public class TicketMasterService : ITicketMasterService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly TicketMaster.APIOptions _options;
    private readonly ILogger<TicketMasterService> _logger;

    public TicketMasterService(ILogger<TicketMasterService> logger,
                               IHttpClientFactory clientFactory, 
                               IOptions<TicketMaster.APIOptions> options)
    {
        _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<AttractionViewModel>> SearchAttractionsAsync(string query, CancellationToken cancellationToken)
    {
        var client = _clientFactory.CreateClient("TicketMaster");
        var url = $"attractions.json?apikey={_options.Key}&keyword={query}&locale=*";
        var results = new List<AttractionViewModel>();

        try
        {
            var response = await client.GetAsync(url, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var root = JsonSerializer.Deserialize<TicketMaster.AttractionRoot>(json);
                var attractions = root?.Embedded?.Attractions;

                if (attractions != null)
                {
                    results = attractions.Select(a => new AttractionViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        ImageUrl = a.Images?.FirstOrDefault()?.Url ?? string.Empty
                    }).ToList();
                }
            }
            else
            {
                _logger.LogWarning("Attraction search failed: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during attraction search");
        }

        return results;
    }

    public async Task<AttractionDetailsViewModel> GetAttractionDetailsAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

		var client = _clientFactory.CreateClient("TicketMaster");
        var detail = new AttractionDetailsViewModel
        {
            Id = id,
            Name = "Unknown",
            ImageUrl = string.Empty,
            ExternalLinks = new(),
        };

        try
        {
            _logger.LogInformation("Fetching details for attraction ID: {Id}", id);

            var json = await client.GetStringAsync($"attractions/{id}.json?apikey={_options.Key}", cancellationToken);
            _logger.LogDebug("Attraction detail JSON received: {Json}", json);

            var attraction = JsonSerializer.Deserialize<TicketMaster.Attraction>(json);
            if (attraction != null)
            {
                detail.Name = attraction.Name ?? "N/A";
                detail.ImageUrl = attraction.Images?.FirstOrDefault()?.Url ?? string.Empty;

                if (attraction.ExternalLinks != null)
                {
                    foreach (var linkGroup in attraction.ExternalLinks)
                    {
                        var key = linkGroup.Key;
                        var url = linkGroup.Value?.FirstOrDefault()?.Url;
                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(url))
                        {
                            detail.ExternalLinks[key] = url;
                        }
                    }
                }

                _logger.LogInformation("Attraction details loaded: {Name}", detail.Name);
            }
            else
            {
                _logger.LogWarning("Deserialized attraction was null for ID: {Id}", id);
            }

            var eventsJson = await client.GetStringAsync($"events.json?apikey={_options.Key}&attractionId={id}&locale=*", cancellationToken);
            _logger.LogDebug("Events JSON received: {Json}", eventsJson);

            var eventRoot = JsonSerializer.Deserialize<TicketMaster.EventRoot>(eventsJson);

            if (eventRoot?.Embedded?.Events != null)
            {
                foreach (var ev in eventRoot.Embedded.Events)
                {
                    detail.Events.Add(new EventViewModel
                    {
                        Id = ev.Id,
                        Name = ev.Name ?? "Unknown Event",
                        Date = ev.Dates?.Start?.LocalDate ?? string.Empty,
                        Venue = ev.Embedded?.Venues?.FirstOrDefault()?.Name ?? string.Empty,
                        ImageUrl = ev.Images?.FirstOrDefault(i => i.Url?.Contains("_EVENT_DETAIL_PAGE") == true)?.Url ?? ev.Images?.FirstOrDefault()?.Url ?? string.Empty
                    });
                }
                _logger.LogInformation("{Count} events loaded for attraction ID: {Id}", detail.Events.Count, id);
            }
            else
            {
                _logger.LogWarning("No events found for attraction ID: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching attraction details for ID: {Id}", id);
        }

        return detail;
    }
}
