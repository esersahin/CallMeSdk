namespace CallMeSdk.DataProviders;

internal sealed class FtpDataProvider
(
    IFtpClientFactory ftpClientFactory
) : BaseDataProvider<FtpConfiguration>
{
    protected override async Task<string> FetchDataAsync()
    {
        var configuration = GetConfiguration();
        
        Validate(configuration);

        using var client = ftpClientFactory.CreateClient(configuration);
        client.Connect();

        await using Stream? ftpStream = client.OpenRead(configuration.FilePath);
        if (ftpStream == null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(ftpStream, Encoding.UTF8);
        var fileContent = await reader.ReadToEndAsync();
        client.Disconnect();
        return fileContent;

    }

    private static void Validate(FtpConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.FtpServer) || 
            string.IsNullOrEmpty(configuration.FilePath) || 
            string.IsNullOrEmpty(configuration.Username) || 
            string.IsNullOrEmpty(configuration.Password))
        {
            throw new InvalidOperationException("FtpServer, FilePath, Username, and Password must be set before making a request.");
        }
    }
}