namespace CallMeSdk.FtpClients;

internal interface IFtpClientFactory
{
    IFtpClient CreateClient(FtpConfiguration configuration);
}

internal sealed class FtpClientFactory
(
    IServiceProvider serviceProvider,
    IFtpClientConfigurator configurator
)
    : IFtpClientFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private readonly IFtpClientConfigurator _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));

    public IFtpClient CreateClient(FtpConfiguration configuration)
    {
        var ftpClient = _serviceProvider.GetKeyedService<IFtpClient>(configuration.ClientType.GetKey())
            ?? throw new InvalidOperationException($"No FTP client found for type: {configuration.ClientType}");

        _configurator.Configure(ftpClient, configuration);
        return ftpClient;
    }
}