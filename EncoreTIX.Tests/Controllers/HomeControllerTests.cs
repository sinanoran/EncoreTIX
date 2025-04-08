using EncoreTIX.Controllers;
using EncoreTIX.Models;
using EncoreTIX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using TicketMaster = EncoreTIX.Models.TicketMaster;

namespace EncoreTIX.Tests.Controllers;

public class HomeControllerTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory = new();
    private readonly Mock<ILogger<HomeController>> _mockLogger = new();
    private readonly Mock<IOptions<TicketMaster.APIOptions>> _mockOptions = new();
    private readonly Mock<ILogger<TicketMasterService>> _mockServiceLogger = new();

    public HomeControllerTests()
    {
        _mockOptions.Setup(o => o.Value).Returns(new TicketMaster.APIOptions { Key = "test-key" });
	}
	[Fact]
	public void Splash_ReturnsView()
	{
        var controller = new HomeController(_mockLogger.Object, Mock.Of<ITicketMasterService>());
        var result = controller.Splash();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Index_ReturnsViewWithAttractions()
    {
        var attractionJson = """
        {
            "_embedded": {
                "attractions": [
                    {
                        "id": "1",
                        "name": "Phish",
                        "images": [{ "url": "https://image" }]
                    }
                ]
            }
        }
        """;

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(attractionJson)
            });

        var client = new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://example.com")
        };

        _mockHttpClientFactory.Setup(f => f.CreateClient("TicketMaster")).Returns(client);

        var service = new TicketMasterService( _mockServiceLogger.Object, _mockHttpClientFactory.Object, _mockOptions.Object);
        var controller = new HomeController(_mockLogger.Object, service);

        var result = await controller.Index("phish", CancellationToken.None);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<AttractionViewModel>>(view.Model);
        Assert.NotNull(model);
        Assert.NotEmpty(model);
        Assert.Equal("Phish", model[0].Name);
    }

    [Fact]
    public async Task Details_ReturnsViewWithDetailModel()
    {
        var attractionJson = """
        {
            "id": "1",
            "name": "Phish",
            "images": [{ "url": "https://image" }],
            "externalLinks": {
                "youtube": [{ "url": "https://youtube.com" }]
            }
        }
        """;

        var eventsJson = """
        {
            "_embedded": {
                "events": [
                    {
                        "id": "101",
                        "name": "Live Show",
                        "dates": { "start": { "localDate": "2025-01-01" } },
                        "images": [{ "url": "https://eventimg_EVENT_DETAIL_PAGE" }],
                        "_embedded": {
                            "venues": [{ "name": "MSG" }]
                        }
                    }
                ]
            }
        }
        """;

        var sequenceHandler = new MockSequenceHttpMessageHandler(new[]
        {
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(attractionJson) },
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(eventsJson) }
        });

        var client = new HttpClient(sequenceHandler)
        {
            BaseAddress = new Uri("https://example.com")
        };

        _mockHttpClientFactory.Setup(f => f.CreateClient("TicketMaster")).Returns(client);

        var service = new TicketMasterService(_mockServiceLogger.Object, _mockHttpClientFactory.Object, _mockOptions.Object);
        var controller = new HomeController(_mockLogger.Object, service);

        var result = await controller.Details("1", CancellationToken.None);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<AttractionDetailsViewModel>(view.Model);
        Assert.Equal("Phish", model.Name);
        Assert.Single(model.Events);
        Assert.Equal("Live Show", model.Events[0].Name);
        Assert.Equal("MSG", model.Events[0].Venue);
    }
}

public class MockSequenceHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses;

    public MockSequenceHttpMessageHandler(IEnumerable<HttpResponseMessage> responses)
    {
        _responses = new Queue<HttpResponseMessage>(responses);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_responses.Count == 0)
        {
            throw new InvalidOperationException("No more responses configured in sequence.");
        }

        return Task.FromResult(_responses.Dequeue());
    }
}
