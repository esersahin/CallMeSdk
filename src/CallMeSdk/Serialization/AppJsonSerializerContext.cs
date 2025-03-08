namespace CallMeSdk.Serialization;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.Unspecified,
    WriteIndented = false)]
[JsonSerializable(typeof(Customer))]
[JsonSerializable(typeof(List<Customer>))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}