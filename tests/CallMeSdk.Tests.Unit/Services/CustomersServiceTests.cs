namespace CallMeSdk.Tests.Unit.Services;

public class CustomersServiceTests
{
    private readonly IDataAdapter _dataAdapter = Substitute.For<IDataAdapter>();
    private readonly IDataProvider<FakeConfiguration> _dataProvider = Substitute.For<IDataProvider<FakeConfiguration>>();
    private readonly IDataProviderFactory _dataProviderFactory = Substitute.For<IDataProviderFactory>();
    private readonly FakeConfiguration _fakeConfiguration = Substitute.For<FakeConfiguration>();
    private readonly CustomersService _customersService;
    
    public CustomersServiceTests()
    {
        _customersService = new CustomersService(_dataProviderFactory);
    }

    [Fact]
    public async Task GetCustomersAsync_ValidData_ReturnsCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { CustomerId = CustomerId.Create("ABC12345", new CustomerIdService()), Name = "John Doe", Email = "john@example.com" }
        };

        _dataProviderFactory.Create<FakeConfiguration>().Returns(_dataProvider);
        _dataProvider.FetchAsync(_dataAdapter).Returns(customers);

        // Act
        var result = await _customersService.GetCustomersAsync(_fakeConfiguration, _dataAdapter);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("John Doe", result.First().Name);
    }
   
    
    public class FakeConfiguration : IClientConfiguration
    {
        public required string BaseUrl { get; init; }
        public required string Endpoint { get; init; }

    }
}