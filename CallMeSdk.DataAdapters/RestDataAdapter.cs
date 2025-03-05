namespace CallMeSdk.DataAdapters;

public sealed class RestDataAdapter
(
    JsonSerializerOptions jsonSerializerOptions
) : IDataAdapter
{
    public IEnumerable<Customer> Adapt(string content)
    {
        if (string.IsNullOrEmpty(content)) 
        {
            throw new InvalidOperationException("content is null or empty.");
        }

        return JsonSerializer.Deserialize<IEnumerable<Customer>>(content, jsonSerializerOptions) ?? [];
    }
}
