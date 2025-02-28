namespace CallMeSdk.Client.WebApi.Resolvers;

public sealed class StrongLifeInsuranceProviderResolver : IProviderResolver
{
    public bool CanHandle(string clientName) => clientName == Clients.StrongLifeInsurance;

    public ICustomerProviderStrategy Resolve(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<CustomerProviderStrategy<FtpConfiguration>>();
    }
}
