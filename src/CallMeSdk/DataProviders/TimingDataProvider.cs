namespace CallMeSdk.DataProviders;

public interface ITimingDataProvider<in TConfiguration> : IDataProvider<TConfiguration>
    where TConfiguration : class, IClientConfiguration
{
    ITimingDataProvider<TConfiguration> SetCorporateName(string corporateName) ;
}

internal sealed class TimingDataProvider<TConfiguration>
(
    IDataProvider<TConfiguration> innerProvider,
    ILogger<TimingDataProvider<TConfiguration>> logger
) : ITimingDataProvider<TConfiguration>
    where TConfiguration : class, IClientConfiguration
{
    private string? _corporateName;

    public IDataProvider<TConfiguration> Configure(TConfiguration configuration)
    {
        return innerProvider.Configure(configuration);
    }

    public ITimingDataProvider<TConfiguration> SetCorporateName(string corporateName)
    {
        _corporateName = corporateName;
        return this;
    }

    public async Task<IEnumerable<Customer>> FetchAsync(IDataAdapter dataAdapter)
    {
        logger.LogInformation("{CorporateName} - {MethodName} started", 
            _corporateName ?? "Unknown", nameof(FetchAsync));
        
        var stopwatch = Stopwatch.StartNew();
        var result = await innerProvider.FetchAsync(dataAdapter);
        stopwatch.Stop();
        
        logger.LogInformation("{CorporateName} - {MethodName} completed in {StopwatchElapsedMilliseconds} ms", 
                _corporateName ?? "Unknown", nameof(FetchAsync), stopwatch.ElapsedMilliseconds);
        
        return result;
    }

}