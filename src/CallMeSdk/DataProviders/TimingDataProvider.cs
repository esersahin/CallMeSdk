namespace CallMeSdk.DataProviders;

public interface ITimingDataProviderStrategy<in TConfiguration> : IDataProviderStrategy<TConfiguration>
    where TConfiguration : class, IClientConfiguration
{
    ITimingDataProviderStrategy<TConfiguration> SetCorporateName(string corporateName) ;
}

internal sealed class TimingDataProvider<TConfiguration>
(
    IDataProviderStrategy<TConfiguration> innerProviderStrategy,
    ILogger<TimingDataProvider<TConfiguration>> logger
) : ITimingDataProviderStrategy<TConfiguration>
    where TConfiguration : class, IClientConfiguration
{
    private string? _corporateName;

    public IDataProviderStrategy<TConfiguration> Configure(TConfiguration configuration)
    {
        return innerProviderStrategy.Configure(configuration);
    }

    public ITimingDataProviderStrategy<TConfiguration> SetCorporateName(string corporateName)
    {
        _corporateName = corporateName;
        return this;
    }

    public async Task<IEnumerable<Customer>> FetchAsync(IDataAdapter dataAdapter)
    {
        logger.LogInformation("{CorporateName} - {MethodName} started", 
            _corporateName ?? "Unknown", nameof(FetchAsync));
        
        var stopwatch = Stopwatch.StartNew();
        var result = await innerProviderStrategy.FetchAsync(dataAdapter);
        stopwatch.Stop();
        
        logger.LogInformation("{CorporateName} - {MethodName} completed in {StopwatchElapsedMilliseconds} ms", 
                _corporateName ?? "Unknown", nameof(FetchAsync), stopwatch.ElapsedMilliseconds);
        
        return result;
    }

}