namespace CallMeSdk.Tests.Integration.Services;

public class CustomersServiceTests
(
    CustomersServiceIntegrationTestFixture fixture
) : IClassFixture<CustomersServiceIntegrationTestFixture>
{
    [Trait("Category", "IntegrationTest")]
    [Fact]
    public async Task GetCustomersAsync_ShouldReturnCustomers_WhenApiReturnsValidData()
    {
        // Arrange
        var customerService = fixture.ServiceProvider.GetRequiredService<CustomersService>();
        var restConfiguration = fixture.RestConfiguration;
        var dataAdapter = fixture.ServiceProvider.GetRequiredService<IDataAdapter>();

        // Act
        var result = await customerService.GetCustomersAsync(restConfiguration, dataAdapter);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Trait("Category", "IntegrationTest")]
    [Fact]
    public async Task GetCustomersAsync_ShouldThrowException_WhenApiReturnsError()
    {
        // Arrange
        var customerService = fixture.ServiceProvider.GetRequiredService<CustomersService>();

        // Simulate error condition using an invalid endpoint
        var invalidRestConfiguration = new RestConfiguration
        {
            BaseUrl = "http://localhost:8080",
            Endpoint = "/api/invalid-endpoint"
        };

        var dataAdapter = fixture.ServiceProvider.GetRequiredService<IDataAdapter>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            customerService.GetCustomersAsync(invalidRestConfiguration, dataAdapter));
    }
}