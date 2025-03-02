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
        var customersService = scope.ServiceProvider.GetRequiredService<ICustomersService>();
        return await customersService.GetCustomersAsync(configuration, adapter);
    }
}