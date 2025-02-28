namespace CallMeSdk.Abstractions;

public interface IDataAdapter
{
    IEnumerable<Customer> Adapt(string content);
}