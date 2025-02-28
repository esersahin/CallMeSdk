namespace CallMeSdk.Client.WebApi.Resolvers;

public interface IProviderResolver
{
    bool CanHandle(string clientName);
    ICustomerProviderStrategy Resolve(IServiceProvider serviceProvider);
}
