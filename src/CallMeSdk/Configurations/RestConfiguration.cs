namespace CallMeSdk.Configurations;

public sealed class RestConfiguration : IClientConfiguration
{
    public required string BaseUrl { get; init; }
    public required string Endpoint { get; init; }
}