namespace CallMeSdk.Tests.Contract.Services;

public class CustomersServiceTests
(
    CustomersServiceContractTestFixture contractTestFixture
) : IClassFixture<CustomersServiceContractTestFixture>
{
    [Trait("Category", "ContractTest")]
    [Fact]
    public async Task GetCustomersAsync_ShouldReturnCustomers_WhenApiReturnsValidData()
    {
        // Arrange
        var httpClientFactory = contractTestFixture.HttpClientFactory;
        var mockDataAdapter = contractTestFixture.MockDataAdapter;
        var customerService = contractTestFixture.ServiceProvider.GetRequiredService<CustomersService>();
        var customerIdService = contractTestFixture.ServiceProvider.GetRequiredService<ICustomerIdService>();
        
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

        var restConfiguration = new RestConfiguration
        {
            BaseUrl = "http://localhost:8080",
            Endpoint = "/api/customers"
        };

        var mockCustomers = new List<Customer>
        {
            new() { CustomerId = CustomerId.Create("ABC12345", customerIdService), Name = "John Doe", Email = "johndoe@abc.com" }
        };
        mockDataAdapter.Adapt(Arg.Any<string>()).Returns(mockCustomers);
        
        // Act
        var result = await customerService.GetCustomersAsync(restConfiguration, mockDataAdapter);
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("John Doe", result.First().Name);
    }
    
    [Trait("Category", "ContractTest")]
    [Fact]
    public async Task GetCustomersAsync_ShouldThrowException_WhenApiReturnsError()
    {
        // Arrange
        var httpClientFactory = contractTestFixture.HttpClientFactory;
        var mockDataAdapter = contractTestFixture.MockDataAdapter;
        var customerService = contractTestFixture.ServiceProvider.GetRequiredService<CustomersService>();

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

        // Simulate error condition using an invalid endpoint
        var restConfiguration = new RestConfiguration
        {
            BaseUrl = "http://localhost:8080",
            Endpoint = "/api/invalid-endpoint"
        };

        mockDataAdapter.Adapt(Arg.Any<string>()).Throws(new InvalidOperationException("API error"));
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            customerService.GetCustomersAsync(restConfiguration, mockDataAdapter));
    }    
}