namespace CallMeSdk.FtpClients;

internal interface IFtpClient : IDisposable
{
    void Configure(FtpConfiguration configuration);
    void Connect();
    void Disconnect();
    Stream? OpenRead(string path);
}