namespace CallMeSdk.Tests.Unit.DomainServices;

public class CustomerIdServicesTests
{
    private readonly CustomerIdService _service = new();

    [Trait("Category", "UnitTest")]
    [Theory]
    [InlineData("ABCDEFGH", true)]   // Valid format
    [InlineData("A1B2C3D4", true)]   // Valid format
    [InlineData(null, false)]        // Null
    [InlineData("", false)]          // Empty
    [InlineData(" ", false)]         // Space
    [InlineData("ABC", false)]       // Too short
    [InlineData("ABCDEFGHI", false)] // Too long
    [InlineData("AB CDEFG", false)]  // Space inside
    [InlineData("AB@CDEFG", false)]  // Special character
    public void IsValidCustomerId_ShouldReturnExpectedResult(string? input, bool expected)
    {
        // Act
        var result = _service.IsValidCustomerId(input);

        // Assert
        Assert.Equal(expected, result);
    }
}