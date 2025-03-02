namespace CallMeSdk.Tests.Integration.Services;

public class CustomersServiceTests
(
    CustomersServiceFixture fixture
) : IClassFixture<CustomersServiceFixture>
{
    [Fact]
    public async Task GetCustomersAsync_ShouldReturnCustomers_WhenApiReturnsValidData()
    {
        // Arrange
        var httpClientFactory = fixture.HttpClientFactory;
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("[{\"CustomerId\":\"ABC12345\",\"Name\":\"John Doe\",\"Email\":\"johndoe@abc.com\"}]")
        };

        var mockHttpMessageHandler = new MockHttpMessageHandler(mockResponse);
        var httpClient = new HttpClient(mockHttpMessageHandler)
        {
            BaseAddress = new Uri("http://localhost:8080")
        };

        httpClientFactory.CreateClient().Returns(httpClient);

        var customerService = fixture.ServiceProvider.GetRequiredService<CustomersService>();

        var restConfiguration = new RestConfiguration
        {
            BaseUrl = "http://localhost:8080",
            Endpoint = "/api/customers"
        };

        var dataAdapter = fixture.ServiceProvider.GetRequiredService<IDataAdapter>();

        // Act
        var result = await customerService.GetCustomersAsync(restConfiguration, dataAdapter);
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("John Doe", result.First().Name);
    }
    
    [Fact]
    public async Task GetCustomersAsync_ShouldThrowException_WhenApiReturnsError()
    {
        // Arrange
        var httpClientFactory = fixture.HttpClientFactory;
        var customerService = fixture.ServiceProvider.GetRequiredService<CustomersService>();

        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Content = new StringContent("Internal Server Error")
        };

        var mockHttpMessageHandler = new MockHttpMessageHandler(mockResponse);
        var httpClient = new HttpClient(mockHttpMessageHandler)
        {
            BaseAddress = new Uri("http://localhost:8080")
        };

        httpClientFactory.CreateClient().Returns(httpClient);

        var restConfiguration = new RestConfiguration
        {
            BaseUrl = "http://localhost:8080",
            Endpoint = "/api/customers"
        };

        var dataAdapter = fixture.ServiceProvider.GetRequiredService<IDataAdapter>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            customerService.GetCustomersAsync(restConfiguration, dataAdapter));
    }    
}