namespace CallMeSdk.Tests.Models;

public class CustomersTests
{
    [Fact]
    public void Create_ValidCustomer_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var customerId = CustomerId.Create("ABC12345", new CustomerIdService());
        const string name = "John Doe";
        const string email = "john@example.com";

        // Act
        var customer = new Customer { CustomerId = customerId, Name = name, Email = email };

        // Assert
        Assert.Equal(customerId, customer.CustomerId);
        Assert.Equal(name, customer.Name);
        Assert.Equal(email, customer.Email);
    }
}