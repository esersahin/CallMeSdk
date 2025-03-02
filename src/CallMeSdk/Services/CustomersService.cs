namespace CallMeSdk.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetCustomersAsync<TConfig>
    (
        TConfig configuration,
        IDataAdapter dataAdapter
    ) where TConfig : class, IClientConfiguration;
}

internal sealed class CustomersService(
    IDataProviderFactory dataProviderFactory
) : ICustomerService
{
    public async Task<IEnumerable<Customer>> GetCustomersAsync<TConfig>(
        TConfig configuration, 
        IDataAdapter dataAdapter) where TConfig : class, IClientConfiguration
    {
        try
        {
            var dataProvider = dataProviderFactory.Create<TConfig>();
            dataProvider.Configure(configuration);
            return await dataProvider.FetchAsync(dataAdapter);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error while fetching data for {typeof(TConfig).Name}", ex);
        }
    }
}