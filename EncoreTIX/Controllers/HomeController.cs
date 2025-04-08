using EncoreTIX.Services;
using Microsoft.AspNetCore.Mvc;

namespace EncoreTIX.Controllers;

public class HomeController : Controller
{
	private readonly ITicketMasterService _ticketMasterService;
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger,
						  ITicketMasterService ticketMasterService)
	{
		_ticketMasterService = ticketMasterService ?? throw new ArgumentNullException(nameof(ticketMasterService));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public IActionResult Splash()
	{
		return View();
	}

	public async Task<IActionResult> Index(string query, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Index action called with query: {Query}", query);
		var results = await _ticketMasterService.SearchAttractionsAsync(query, cancellationToken);
		ViewData["Query"] = query;
		return View(results);
	}

	public async Task<IActionResult> Details(string id, CancellationToken cancellationToken)
	{
		var details = await _ticketMasterService.GetAttractionDetailsAsync(id, cancellationToken);
		return View(details);
	}
}
