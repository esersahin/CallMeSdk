namespace CallMeSdk.FtpClients;

internal sealed class FluentFtpClient : IFtpClient
{
    private FtpClient? _ftpClient;

    public void Configure(FtpConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _ftpClient = new FtpClient(configuration.FtpServer, configuration.Username, configuration.Password);
    }

    public void Connect()
    {
        _ftpClient?.Connect();
    }

    public void Disconnect()
    {
        _ftpClient?.Disconnect();
    }

    public Stream? OpenRead(string path)
    {
        return _ftpClient?.OpenRead(path);
    }

    public void Dispose()
    {
        _ftpClient?.Dispose();
    }
}