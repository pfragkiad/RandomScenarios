using Moq.Protected;
using Moq;
using System.Net;

namespace MoqHttpClient.Extensions;

public static class Factory
{
    //https://github.com/Moq/moq4/wiki/Quickstart

    public static Mock<IHttpClientFactory> GetMockHttpClientFactory(
      Dictionary<string, string> clientNameToContent,
      Dictionary<string, string?>? clientNameToBaseAddress = null)
    {
        var mockFactory = new Mock<IHttpClientFactory>();

        foreach (var entry in clientNameToContent)
        {
            string clientName = entry.Key;
            string cachedContent = entry.Value; // File.ReadAllText(entry.Value);

            string? baseAddress = clientNameToBaseAddress?.GetValueOrDefault(clientName);

            // Create a mock HttpMessageHandler that returns a predefined response
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(cachedContent),
                });

            // Create a HttpClient with the mocked handler
            var httpClient =
                !string.IsNullOrWhiteSpace(baseAddress) ?
                    new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri(baseAddress) } :
                    new HttpClient(mockHttpMessageHandler.Object);

            // Create a mock IHttpClientFactory that returns the HttpClient
            mockFactory.Setup(_ => _.CreateClient(clientName)).Returns(httpClient);
        }

        return mockFactory;
    }


    public static Mock<IHttpClientFactory> GetMockHttpClientFactory(IEnumerable<HttpClientContent> clients)
        => GetMockHttpClientFactory(
            clients.ToDictionary(c => c.ClientName, c => c.Content),
            clients.ToDictionary(c => c.ClientName, c => c.BaseAddress));
}
