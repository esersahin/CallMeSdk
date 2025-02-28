namespace CallMeSdk.DataProviders;

public interface IDataProviderFactory
{
    IDataProviderStrategy<TConfig> Create<TConfig>() where TConfig : class, IClientConfiguration;
}

internal sealed class DataProviderFactory(IServiceProvider serviceProvider) : IDataProviderFactory
{
    public IDataProviderStrategy<TConfiguration> Create<TConfiguration>() where TConfiguration : class, IClientConfiguration
    {
        var strategy =  serviceProvider.GetRequiredService<IDataProviderStrategy<TConfiguration>>();
        
        if (strategy is null)
        {
            throw new InvalidOperationException($"No service registered for {strategy}.");
        }
        
        return strategy;
    }
}