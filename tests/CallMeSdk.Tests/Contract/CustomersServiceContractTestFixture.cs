namespace CallMeSdk.Tests.Contract;

public sealed class CustomersServiceContractTestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }
    public IHttpClientFactory HttpClientFactory { get; private set; }
    public IDataAdapter MockDataAdapter { get; private set; }

    public CustomersServiceContractTestFixture()
    {
        var services = new ServiceCollection();

        // Create HttpClientFactory mock and add to DI
        HttpClientFactory = Substitute.For<IHttpClientFactory>();
        services.AddSingleton(HttpClientFactory);        
        
        // Create IDataAdapter mock and add to DI
        MockDataAdapter = Substitute.For<IDataAdapter>();
        services.AddSingleton(MockDataAdapter);
        
        // Add other items to DI
        services.AddSingleton<IDataProviderFactory, DataProviderFactory>().
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