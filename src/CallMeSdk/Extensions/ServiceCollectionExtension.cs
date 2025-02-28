namespace CallMeSdk.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCallMeSdk(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddSingleton<IDataProviderFactory, DataProviderFactory>();
        
        services.AddTransient<IDataProvider<SoapConfiguration>, SoapDataProvider>().
                 AddTransient<IDataProvider<RestConfiguration>, RestDataProvider>().
                 AddTransient<IDataProvider<FtpConfiguration>, FtpDataProvider>();
        
        services.AddSingleton<ICustomerIdService, CustomerIdService>().
                 AddTransient<ICustomerService, CustomerService>();

        services.AddTransient<ICompositeDataProvider, CompositeDataProvider>().
                 AddTransient(typeof(ITimingDataProvider<>), typeof(TimingDataProvider<>));

        services.AddSingleton<IFtpClientConfigurator, FtpClientConfigurator>().
                 AddSingleton<IFtpClientFactory, FtpClientFactory>().
                 AddKeyedTransient<IFtpClient, FluentFtpClient>(FtpClientType.FluentFtp.GetKey());
        
        services.AddScoped<JsonSerializerOptions>(provider =>
        {
            var customerIdService = provider.GetRequiredService<ICustomerIdService>();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CustomerIdJsonConverter(customerIdService));
            return options;
        });
        
        return services;
    }
}