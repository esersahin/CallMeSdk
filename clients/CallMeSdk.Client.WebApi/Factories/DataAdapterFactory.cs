namespace CallMeSdk.Client.WebApi.Factories;

public interface IDataAdapterFactory
{
    IDataAdapter Create(string providerName);
}

public class DataAdapterFactory(IServiceProvider serviceProvider) : IDataAdapterFactory
{
    public IDataAdapter Create(string providerName)
    {
        return serviceProvider.GetRequiredKeyedService<IDataAdapter>(providerName);
    }
}