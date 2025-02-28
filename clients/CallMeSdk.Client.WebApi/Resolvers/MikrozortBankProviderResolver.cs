namespace CallMeSdk.Client.WebApi.Resolvers;

public sealed class MikrozortBankProviderResolver : IProviderResolver
{
    public bool CanHandle(string clientName) => clientName == Clients.MikrozortBank;

    public ICustomerProviderStrategy Resolve(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<CustomerProviderStrategy<RestConfiguration>>();
    }
}
