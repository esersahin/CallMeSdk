using CallMeSdk.Serialization;

namespace CallMeSdk.Client.DataAdapters;

public sealed class RestDataAdapter
(
    AppJsonSerializerContext context
) : IDataAdapter
{
    public IEnumerable<Customer> Adapt(string content)
    {
        if (string.IsNullOrEmpty(content)) 
        {
            throw new InvalidOperationException("content is null or empty.");
        }

        return JsonSerializer.Deserialize(content, context.ListCustomer) ?? [];
    }
}
