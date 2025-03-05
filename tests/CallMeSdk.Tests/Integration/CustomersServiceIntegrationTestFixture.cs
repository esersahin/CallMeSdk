
namespace CallMeSdk.Tests.Integration;

public sealed class CustomersServiceIntegrationTestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }
    public IHttpClientFactory HttpClientFactory { get; private set; }
    public RestConfiguration RestConfiguration { get; private set; }

    public CustomersServiceIntegrationTestFixture()
    {
        var services = new ServiceCollection();

        services.AddHttpClient();

        RestConfiguration = new RestConfiguration
        {
            BaseUrl = "http://localhost:8080",
            Endpoint = "/api/customers/rest"
        };

        services.AddSingleton<IDataAdapter, RestDataAdapter>();

        services.AddSingleton<IDataProviderFactory, DataProviderFactory>();
        services.AddSingleton<ICustomerIdService, CustomerIdService>();

        services.AddTransient<IDataProvider<SoapConfiguration>, SoapDataProvider>().
                 AddTransient<IDataProvider<RestConfiguration>, RestDataProvider>().
                 AddTransient<IDataProvider<FtpConfiguration>, FtpDataProvider>().
                 AddTransient<CustomersService>();

        services.AddSingleton<JsonSerializerOptions>(provider =>
        {
            var customerIdService = provider.GetRequiredService<ICustomerIdService>();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CustomerIdJsonConverter(customerIdService));
            return options;
        });

        ServiceProvider = services.BuildServiceProvider();

        HttpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();
    }

    public void Dispose()
    {
        ServiceProvider?.Dispose();
    }
}