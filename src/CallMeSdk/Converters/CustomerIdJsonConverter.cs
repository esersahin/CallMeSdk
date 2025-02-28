namespace CallMeSdk.Converters;

internal sealed class CustomerIdJsonConverter
(
    ICustomerIdService customerIdService
) : JsonConverter<CustomerId>
{
    public override CustomerId Read(ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            throw new JsonException("CustomerId cannot be null or empty.");

        return CustomerId.Create(value, customerIdService);
    }

    public override void Write(Utf8JsonWriter writer, 
        CustomerId value, 
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
