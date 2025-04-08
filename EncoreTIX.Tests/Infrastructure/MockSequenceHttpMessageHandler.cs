using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EncoreTIX.Tests.Infrastructure;

internal class MockSequenceHttpMessageHandler : HttpMessageHandler
{
	private readonly Queue<HttpResponseMessage> _responses;

	internal MockSequenceHttpMessageHandler(IEnumerable<HttpResponseMessage> responses)
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