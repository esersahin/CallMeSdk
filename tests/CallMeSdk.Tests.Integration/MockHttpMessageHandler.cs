namespace CallMeSdk.Tests.Integration.DataProviders;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _mockResponse;

    public MockHttpMessageHandler(HttpResponseMessage mockResponse)
    {
        _mockResponse = mockResponse;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_mockResponse);
    }
}