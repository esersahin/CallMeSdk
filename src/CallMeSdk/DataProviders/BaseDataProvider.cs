namespace CallMeSdk.DataProviders;

internal abstract class BaseDataProvider<TConfiguration> : IDataProvider<TConfiguration>
    where TConfiguration : class, IClientConfiguration
{
    private TConfiguration? _configuration;

    public IDataProvider<TConfiguration> Configure(TConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }
    
    protected TConfiguration GetConfiguration()
    {
        if (_configuration is null)
        {
            throw new InvalidOperationException("Configuration must be set before using the provider.");    
        }

        return _configuration;
    }

    public async Task<IEnumerable<Customer>> FetchAsync(IDataAdapter dataAdapter)
    {
        var rawData = await FetchDataAsync();
        return dataAdapter.Adapt(rawData);
    }

    protected abstract Task<string> FetchDataAsync();
}