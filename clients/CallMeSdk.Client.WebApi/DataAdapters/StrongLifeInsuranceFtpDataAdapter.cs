namespace CallMeSdk.Client.WebApi.DataAdapters;

public class StrongLifeInsuranceFtpDataAdapter
(
    ICustomerIdService customerIdService
) : IDataAdapter
{
    public IEnumerable<Customer> Adapt(string content)
    {
        var customers = new List<Customer>();
        var lines = content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            string?[] columns = line.Split(',');
            customers.Add(new Customer{
                CustomerId = CustomerId.Create(columns[0],customerIdService),
                Name = columns[1],
                Email = columns[2]
            });
        }

        return customers;
    }
}