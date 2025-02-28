namespace CallMeSdk.Client.WebApi.Strategies;

public interface ICustomerProviderStrategy
{
    Task<IEnumerable<Customer>> GetCustomersAsync(string clientName);
}

public sealed class CustomerProviderStrategy<TConfiguration>(
    ICustomerService customerService,
    IConfigurationResolver configurationResolver,
    IDataAdapterFactory adapterFactory
) : ICustomerProviderStrategy where TConfiguration : class, IClientConfiguration
{
    public async Task<IEnumerable<Customer>> GetCustomersAsync(string clientName)
    {
        var config = configurationResolver.GetConfiguration<TConfiguration>(clientName);
        var adapter = adapterFactory.Create(clientName);
        return await customerService.GetCustomersAsync(config, adapter);
    }
}