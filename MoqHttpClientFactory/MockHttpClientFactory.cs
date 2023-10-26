using Moq.Protected;
using Moq;
using System.Net;

namespace MoqHttpClient.Extensions;

public static class MockHttpClientFactory
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

            //Nice explanation of the protected approach here:
            //https://www.youtube.com/watch?v=7OFZZAHGv9o 

            // Create a mock HttpMessageHandler that returns a predefined response
            Mock<HttpMessageHandler> mockHttpMessageHandler = GetMockHttpMessageHandler(cachedContent);

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

    private static Mock<HttpMessageHandler> GetMockHttpMessageHandler(string cachedContent)
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(), //ItExpr.Is<HttpRequestMessage>(x=> x.RequestUri== new Uri("http://www.random.org"))
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(cachedContent),
            });
        return mockHttpMessageHandler;
    }

    public static Mock<IHttpClientFactory> GetMockHttpClientFactory(IEnumerable<HttpClientContent> clients)
        => GetMockHttpClientFactory(
            clients.ToDictionary(c => c.ClientName, c => c.Content),
            clients.ToDictionary(c => c.ClientName, c => c.BaseAddress));
}
