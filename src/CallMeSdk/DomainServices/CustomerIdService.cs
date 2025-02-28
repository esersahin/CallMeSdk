namespace CallMeSdk.DomainServices;

public interface ICustomerIdService
{
    bool IsValidCustomerId(string? customerId);
}

public class CustomerIdService : ICustomerIdService
{
    public bool IsValidCustomerId(string? customerId)
    {
        return !string.IsNullOrWhiteSpace(customerId) &&
            customerId.Length == 8 &&
            customerId.All(char.IsLetterOrDigit);
    }
}