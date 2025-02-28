namespace CallMeSdk.FtpClients;

internal interface IFtpClientFactory
{
    IFtpClient CreateClient(FtpConfiguration configuration);
}

internal sealed class FtpClientFactory
(
    IServiceProvider serviceProvider,
    IFtpClientConfigurator configurator
) : IFtpClientFactory
{
    public IFtpClient CreateClient(FtpConfiguration configuration)
    {
        var ftpClient = serviceProvider.GetKeyedService<IFtpClient>(configuration.ClientType.GetKey())
            ?? throw new InvalidOperationException($"No FTP client found for type: {configuration.ClientType}");

        configurator.Configure(ftpClient, configuration);
        return ftpClient;
    }
}