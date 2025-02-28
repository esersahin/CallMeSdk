namespace CallMeSdk.DataProviders;

internal sealed class SoapDataProvider
(
    IHttpClientFactory httpClientFactory
) : BaseDataProvider<SoapConfiguration>
{
    protected override async Task<string> FetchDataAsync()
    {
        const string soapRequest = 
        """
           <?xml version="1.0"?>
           <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/">
               <SOAP-ENV:Body>
               </SOAP-ENV:Body>
           </SOAP-ENV:Envelope>
        """;

        var configuration = GetConfiguration();

        Validate(configuration);
        
        var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
        var url = $"{configuration.BaseUrl}{configuration.Endpoint}";
        
        var httpClient = httpClientFactory.CreateClient();
        
        var response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
    
    private static void Validate(SoapConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.BaseUrl) || 
            string.IsNullOrEmpty(configuration.Endpoint))
        {
            throw new InvalidOperationException("BaseUrl and Endpoint must be set before making a request.");
        }
    }
}