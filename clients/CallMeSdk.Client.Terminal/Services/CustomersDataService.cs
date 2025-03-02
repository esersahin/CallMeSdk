namespace CallMeSdk.Client.Terminal.Services;

public interface ICustomersDataService
{
    Task RetrieveAndPrintCustomersAsync();
}

public sealed class CustomersDataService(
    ICustomersService customerService,
    IConfigurationResolver configurationResolver,
    IDataAdapterFactory dataAdapterFactory
) : ICustomersDataService
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
