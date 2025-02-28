namespace CallMeSdk.Client.Terminal.DataAdapters;

public interface IDataAdapterFactory
{
    IDataAdapter Create(string providerName);
}

public sealed class DataAdapterFactory(IServiceProvider serviceProvider) : IDataAdapterFactory
{
    public IDataAdapter Create(string providerName)
    {
        return serviceProvider.GetRequiredKeyedService<IDataAdapter>(providerName);
    }
}