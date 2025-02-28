namespace CallMeSdk.FtpClients;

public enum FtpClientType
{
    FluentFtp,
}

internal static class FtpClientTypeHelper
{
    private const string FluentFtpKey = "FluentFtp";

    public static string GetKey(this FtpClientType clientType)
    {
        return clientType switch
        {
            FtpClientType.FluentFtp => FluentFtpKey,
            _                       => throw new ArgumentException($"FtpClientType '{clientType}' is not supported.")
        };
    }
}