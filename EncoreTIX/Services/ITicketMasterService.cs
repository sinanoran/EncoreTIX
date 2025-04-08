using EncoreTIX.Models;

namespace EncoreTIX.Services;

public interface ITicketMasterService
{
	Task<List<AttractionViewModel>> SearchAttractionsAsync(string query, CancellationToken cancellationToken);
	Task<AttractionDetailsViewModel> GetAttractionDetailsAsync(string id, CancellationToken cancellationToken);
}

