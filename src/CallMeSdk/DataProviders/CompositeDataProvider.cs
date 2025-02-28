namespace CallMeSdk.DataProviders;

public interface ICompositeDataProvider
{
    Task<IEnumerable<Customer>> FetchAsync();

    void AddProvider<TConfiguration>
    (
        IDataProviderStrategy<TConfiguration> providerStrategy,
        TConfiguration configuration,
        IDataAdapter strategy
    ) where TConfiguration : class, IClientConfiguration;
}

internal sealed class CompositeDataProvider : ICompositeDataProvider
{
    private readonly List<IProviderWrapper> _providers = new();

    public void AddProvider<TConfiguration>(
        IDataProviderStrategy<TConfiguration> providerStrategy,
        TConfiguration configuration,
        IDataAdapter strategy) where TConfiguration : class, IClientConfiguration
    {
        providerStrategy.Configure(configuration);
        _providers.Add(new ProviderWrapper(providerStrategy, strategy));
    }

    public async Task<IEnumerable<Customer>> FetchAsync()
    {
        var results = new List<Customer>();

        foreach (var providerWrapper in _providers)
        {
            var data = await providerWrapper.FetchAsync();
            results.AddRange(data);
        }

        return results;
    }

    private interface IProviderWrapper
    {
        Task<IEnumerable<Customer>> FetchAsync();
    }

    private sealed class ProviderWrapper
    (
        IDataProviderStrategy providerStrategy,
        IDataAdapter strategy
    ) : IProviderWrapper
    {
        public async Task<IEnumerable<Customer>> FetchAsync()
        {
            return await providerStrategy.FetchAsync(strategy);
        }
    }
}