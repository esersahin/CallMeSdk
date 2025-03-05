namespace CallMeSdk.Tests.Unit.ValueObjects;

public class CustomerIdTests
{
    private readonly ICustomerIdService _customerIdService = new CustomerIdService();

    [Trait("Category", "UnitTest")]
    [Fact]
    public void Create_ValidCustomerId_ShouldReturnCustomerId()
    {
        // Arrange
        var validId = "ABC12345";

        // Act
        var customerId = CustomerId.Create(validId, _customerIdService);

        // Assert
        Assert.NotNull(customerId);
        Assert.Equal(validId, customerId.Value);
    }

    [Trait("Category", "UnitTest")]
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyCustomerId_ShouldThrowArgumentException(string? invalidId)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CustomerId.Create(invalidId, _customerIdService));
    }

    [Trait("Category", "UnitTest")]
    [Theory]
    [InlineData("123")]       // Too short
    [InlineData("123456789")] // Too long
    [InlineData("ABC 1234")]  // Contains space
    [InlineData("ABC@1234")]  // Contains special character
    public void Create_InvalidFormatCustomerId_ShouldThrowArgumentException(string invalidId)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => CustomerId.Create(invalidId, _customerIdService));
    } 
}