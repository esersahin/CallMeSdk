namespace CallMeSdk.Client.WebApi.Strategies;

public interface ICustomerProviderStrategy
{
    Task<IEnumerable<Customer>> GetCustomersAsync(string clientName);
}

public sealed class CustomerProviderStrategy<TConfiguration>(
    ICustomersService customersService,
    IConfigurationResolver configurationResolver,
    IDataAdapterFactory adapterFactory
) : ICustomerProviderStrategy where TConfiguration : class, IClientConfiguration
{
    public async Task<IEnumerable<Customer>> GetCustomersAsync(string clientName)
    {
        var config = configurationResolver.GetConfiguration<TConfiguration>(clientName);
        var adapter = adapterFactory.Create(clientName);
        return await customersService.GetCustomersAsync(config, adapter);
    }
}