namespace CallMeSdk.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetCustomersAsync<TConfig>
    (
        TConfig configuration,
        IDataAdapter dataAdapter
    ) where TConfig : class, IClientConfiguration;
}

internal sealed class CustomerService(
    IDataProviderFactory dataProviderFactory
) : ICustomerService
{
    public async Task<IEnumerable<Customer>> GetCustomersAsync<TConfig>(
        TConfig configuration, 
        IDataAdapter dataAdapter) where TConfig : class, IClientConfiguration
    {
        try
        {
            return await dataProviderFactory.
                Create<TConfig>().
                Configure(configuration).
                FetchAsync(dataAdapter);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error while fetching data for {typeof(TConfig).Name}", ex);
        }
    }
}