namespace CallMeSdk.Tests.Integration.DataProviders;

public sealed class CustomersServiceFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }
    public IHttpClientFactory HttpClientFactory { get; private set; }

    public CustomersServiceFixture()
    {
        var services = new ServiceCollection();

        HttpClientFactory = Substitute.For<IHttpClientFactory>();
        services.AddSingleton(HttpClientFactory);        
        
        services.AddSingleton<IDataProviderFactory, DataProviderFactory>().
                 AddSingleton<IDataAdapter, MikrozortBankRestDataAdapter>().
                 AddSingleton<ICustomerIdService, CustomerIdService>();
        
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
    }
    
    public void Dispose() => ServiceProvider?.Dispose();
}