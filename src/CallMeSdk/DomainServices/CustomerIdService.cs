namespace CallMeSdk.DomainServices;

public interface ICustomerIdService
{
    bool IsValidCustomerId(string? customerId);
}

public class CustomerIdService : ICustomerIdService
{
    public bool IsValidCustomerId(string? customerId)
    {
        // Example validation logic: Must be exactly 8 alphanumeric characters
        return !string.IsNullOrWhiteSpace(customerId) &&
            customerId.Length == 8 &&
            customerId.All(char.IsLetterOrDigit);
    }
}