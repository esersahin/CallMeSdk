namespace CallMeSdk.Client.Terminal.Strategies;

public interface ICustomerProviderStrategy
{ 
    Task<IEnumerable<Customer>> FetchCustomersAsync(IServiceScope scope, string clientName, IDataAdapter adapter);
}

public abstract class BaseStrategy<TConfig> : ICustomerProviderStrategy where TConfig : class, IClientConfiguration
{
    public async Task<IEnumerable<Customer>> FetchCustomersAsync(IServiceScope scope, string clientName, IDataAdapter adapter)
    {
        var configuration = scope.GetConfiguration<TConfig>(clientName);
        var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
        return await customerService.GetCustomersAsync(configuration, adapter);
    }
}