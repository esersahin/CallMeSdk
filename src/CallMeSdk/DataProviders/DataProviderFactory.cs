namespace CallMeSdk.DataProviders;

public interface IDataProviderFactory
{
    IDataProvider<TConfig> Create<TConfig>() where TConfig : class, IClientConfiguration;
}

internal sealed class DataProviderFactory(IServiceProvider serviceProvider) : IDataProviderFactory
{
    public IDataProvider<TConfiguration> Create<TConfiguration>() where TConfiguration : class, IClientConfiguration
    {
        var strategy =  serviceProvider.GetRequiredService<IDataProvider<TConfiguration>>();
        
        if (strategy is null)
        {
            throw new InvalidOperationException($"No service registered for {strategy}.");
        }
        
        return strategy;
    }
}