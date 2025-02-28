namespace CallMeSdk.DataProviders;

public interface IDataProviderStrategy
{
    Task<IEnumerable<Customer>> FetchAsync(IDataAdapter dataAdapter);
}

public interface IDataProviderStrategy<in TConfiguration> : IDataProviderStrategy
    where TConfiguration : class, IClientConfiguration
{
    IDataProviderStrategy<TConfiguration> Configure(TConfiguration configuration);
}