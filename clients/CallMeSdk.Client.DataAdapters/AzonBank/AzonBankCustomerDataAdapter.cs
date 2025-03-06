namespace CallMeSdk.Client.DataAdapters.AzonBank;

public sealed class AzonBankCustomerDataAdapter
(
    ICustomerIdService customerIdService
) : IDataAdapter
{
    public IEnumerable<Customer> Adapt(string content)
    {
        return new List<AzonBankCustomer>(3) { 
            new()
            {
                TotalPolicyAmount = 1000,
                CustomerId = CustomerId.Create("AZN00001", customerIdService) ,
                Name = "AZN00001",
                Email = "azoncustomer001@azonbank.com"
            },
            new()
            {
                TotalPolicyAmount = 2000,
                CustomerId = CustomerId.Create("AZN00002", customerIdService) ,
                Name = "AZN00002",
                Email = "azoncustomer002@azonbank.com"
            },
            new()
            {
                TotalPolicyAmount = 3000,
                CustomerId = CustomerId.Create("AZN00003", customerIdService) ,
                Name = "AZN00003",
                Email = "azoncustomer003@azonbank.com"
            },
        };
    }
}