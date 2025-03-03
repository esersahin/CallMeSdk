namespace CallMeSdk.Tests.Integration;

public class MockHttpMessageHandler
(
    HttpResponseMessage mockResponse
) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(mockResponse);
    }
}