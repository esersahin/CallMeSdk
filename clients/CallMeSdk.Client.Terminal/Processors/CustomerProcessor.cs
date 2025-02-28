namespace CallMeSdk.Client.Terminal.Processors;

public interface ICustomerProcessor
{
    Task ProcessCustomersAsync();
}

public sealed class CustomerProcessor
(
    IServiceProvider serviceProvider,
    IDataAdapterFactory dataAdapterFactory
) : ICustomerProcessor
{
    public async Task ProcessCustomersAsync()
    {
        var clients = new[] 
        { 
            Clients.AzonBank, 
            Clients.MikrozortBank, 
            Clients.StrongLifeInsurance 
        };

        using var scope = serviceProvider.CreateScope();
        foreach (var clientName in clients)
        {
            await ProcessCustomerProviderAsync(scope, clientName);
        }
    }

    private async Task ProcessCustomerProviderAsync(IServiceScope scope, string clientName)
    {
        var strategy = scope.ServiceProvider.GetRequiredKeyedService<ICustomerProviderStrategy>(clientName);

        if (strategy == null)
        {
            throw new InvalidOperationException($"No strategy found for client: {clientName}");
        }

        var adapter = dataAdapterFactory.Create(clientName);
        var customers = await strategy.FetchCustomersAsync(scope, clientName, adapter);
        customers.PrintCustomers(clientName);
    }
}
