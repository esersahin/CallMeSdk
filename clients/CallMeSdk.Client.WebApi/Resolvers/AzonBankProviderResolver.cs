namespace CallMeSdk.Client.WebApi.Resolvers;

public sealed class AzonBankProviderResolver : IProviderResolver
{
    public bool CanHandle(string clientName) => clientName == Clients.AzonBank;

    public ICustomerProviderStrategy Resolve(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<CustomerProviderStrategy<SoapConfiguration>>();
    }
}
