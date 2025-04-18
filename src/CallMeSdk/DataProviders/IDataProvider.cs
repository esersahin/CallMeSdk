namespace CallMeSdk.DataProviders;

public interface IDataProvider
{
    Task<IEnumerable<Customer>> FetchAsync(IDataAdapter dataAdapter);
}

public interface IDataProvider<in TConfiguration> : IDataProvider
    where TConfiguration : class, IClientConfiguration
{
    void Configure(TConfiguration configuration);
}