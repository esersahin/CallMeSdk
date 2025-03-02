namespace CallMeSdk.Client.Terminal.Extensions;

public static class CustomersExtension
{
    public static void PrintCustomers(this IEnumerable<Customer> customers, string corporateName)
    {
        Console.WriteLine($"{corporateName} - Data");
        Console.WriteLine(new string('-', corporateName.Length + 7));

        foreach (var customer in customers)
        {
            Console.WriteLine($"CustomerId: {customer.CustomerId}, Name: {customer.Name}, Email: {customer.Email}");
        }

        Console.WriteLine();
    }
}