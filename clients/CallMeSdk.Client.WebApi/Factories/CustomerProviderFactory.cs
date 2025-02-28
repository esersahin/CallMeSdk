namespace CallMeSdk.Client.WebApi.Factories;

public interface ICustomerProviderFactory
{
    ICustomerProviderStrategy Create(string clientName);
}

public sealed class CustomerProviderFactory(
    IEnumerable<IProviderResolver> resolvers, 
    IServiceProvider serviceProvider) : ICustomerProviderFactory
{
    public ICustomerProviderStrategy Create(string clientName)
    {
        var resolver = resolvers.FirstOrDefault(r => r.CanHandle(clientName));
        if (resolver == null)
        {
            throw new ArgumentException($"Unsupported client: {clientName}");
        }

        return resolver.Resolve(serviceProvider);
    }
}