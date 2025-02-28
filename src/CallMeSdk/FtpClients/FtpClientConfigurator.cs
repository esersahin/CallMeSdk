namespace CallMeSdk.FtpClients;

internal interface IFtpClientConfigurator
{
    void Configure(IFtpClient client, FtpConfiguration ftpConfiguration);
}

internal sealed class FtpClientConfigurator : IFtpClientConfigurator
{
    public void Configure(IFtpClient client, FtpConfiguration ftpConfiguration)
    {
        switch (client)
        {
            case FluentFtpClient fluentClient:
                fluentClient.Configure(ftpConfiguration);
                break;
            default:
                throw new InvalidOperationException("Unsupported FTP client.");
        }
    }
}