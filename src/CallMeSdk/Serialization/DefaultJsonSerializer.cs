namespace CallMeSdk.Serialization;

public class DefaultJsonSerializer : IJsonSerializer
{
    private readonly AppJsonSerializerContext _context;

    public DefaultJsonSerializer()
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        _context = new AppJsonSerializerContext(options);
    }

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, typeof(T), _context);
    }

    public T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _context.Options);
    }
}