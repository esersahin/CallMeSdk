namespace CallMeSdk.Configurations;

public sealed class FtpConfiguration : IClientConfiguration
{
    public required string FtpServer { get; init; }
    public required string FilePath { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required FtpClientType ClientType { get; init; }
}

