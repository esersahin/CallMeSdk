namespace CallMeSdk.Client.WebApi.DataAdapters;

public class AzonBankSoapDataAdapter
(
    ICustomerIdService customerIdService
) : IDataAdapter
{
    public IEnumerable<Customer> Adapt(string content)
    {
        var serializer = new XmlSerializer(typeof(SoapEnvelope));
        using var reader = new StringReader(content);
        
        if (serializer.Deserialize(reader) is not SoapEnvelope envelope)
        {
            return [];
        }

        var customerDtos = envelope.SoapBody?.SoapCustomers?.CustomerList ?? [];

        var customers = new List<Customer>();
        foreach (var dto in customerDtos)
        {
            var customer = new Customer
            {
                CustomerId = CustomerId.Create(dto.CustomerId, customerIdService),
                Name = dto.Name,
                Email = dto.Email
            };
            customers.Add(customer);
        }
        return customers;

    }
}