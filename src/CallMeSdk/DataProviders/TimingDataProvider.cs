namespace CallMeSdk.DataProviders;

public interface ITimingDataProvider<in TConfiguration> : IDataProvider<TConfiguration>
    where TConfiguration : class, IClientConfiguration
{
    ITimingDataProvider<TConfiguration> SetCorporateName(string corporateName) ;
}

internal sealed class TimingDataProvider<TConfiguration>
(
    IDataProvider<TConfiguration> innerProvider,
    ILogger<TimingDataProvider<TConfiguration>> logger,
    IStopwatchService stopwatchService
) : ITimingDataProvider<TConfiguration>
    where TConfiguration : class, IClientConfiguration
{
    private string? _corporateName;

    public void Configure(TConfiguration configuration)
    {
        innerProvider.Configure(configuration);
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

        stopwatchService.Start();
        var result = await innerProvider.FetchAsync(dataAdapter);
        stopwatchService.Stop();
        
        logger.LogInformation("{CorporateName} - {MethodName} completed in {StopwatchElapsedMilliseconds} ms", 
                _corporateName ?? "Unknown", nameof(FetchAsync), stopwatchService.ElapsedMilliseconds);
        
        return result;
    }

}