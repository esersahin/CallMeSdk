namespace CallMeSdk.Client.Terminal.Services;

public interface ICustomerDataService
{
    Task RetrieveAndPrintCustomersAsync();
}

public sealed class CustomerDataService(
    ICustomerService customerService,
    IConfigurationResolver configurationResolver,
    IDataAdapterFactory dataAdapterFactory
) : ICustomerDataService
{
    public async Task RetrieveAndPrintCustomersAsync()
    {
        var tasks = new List<Task>
        {
            RetrieveAndPrintCustomersForProviderAsync<SoapConfiguration>(Clients.AzonBank),
            RetrieveAndPrintCustomersForProviderAsync<RestConfiguration>(Clients.MikrozortBank),
            RetrieveAndPrintCustomersForProviderAsync<FtpConfiguration>(Clients.StrongLifeInsurance)
        };

        await Task.WhenAll(tasks);
    }

    private async Task RetrieveAndPrintCustomersForProviderAsync<TConfiguration>(string clientName)
        where TConfiguration : class, IClientConfiguration
    {
        var config = configurationResolver.GetConfiguration<TConfiguration>(clientName);
        var adapter = dataAdapterFactory.Create(clientName);
        var customers = await customerService.GetCustomersAsync(config, adapter);
        customers.PrintCustomers(clientName);
    }
}
