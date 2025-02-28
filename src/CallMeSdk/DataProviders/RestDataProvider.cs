namespace CallMeSdk.DataProviders;

internal sealed class RestDataProvider
(
    IHttpClientFactory httpClientFactory
) : BaseDataProvider<RestConfiguration>
{
    protected override async Task<string> FetchDataAsync()
    {
        var configuration = GetConfiguration();
        
        Validate(configuration);
        
        var httpClient = httpClientFactory.CreateClient();

        var url = $"{configuration.BaseUrl}{configuration.Endpoint}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadAsStringAsync();
    }

    private static void Validate(RestConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.BaseUrl) || 
            string.IsNullOrEmpty(configuration.Endpoint))
        {
            throw new InvalidOperationException("BaseUrl and Endpoint must be set before making a request.");
        }
    }
}