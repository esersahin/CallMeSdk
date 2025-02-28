namespace CallMeSdk.Client.WebApi.Services;

public interface ICustomerProviderService
{
    Task<IEnumerable<Customer>> GetCustomersAsync(string clientName);
}

public sealed class CustomerProviderService(
    ICustomerProviderFactory providerFactory
) : ICustomerProviderService
{
    public async Task<IEnumerable<Customer>> GetCustomersAsync(string clientName)
    {
        var strategy = providerFactory.Create(clientName);
        return await strategy.GetCustomersAsync(clientName);
    }
}



