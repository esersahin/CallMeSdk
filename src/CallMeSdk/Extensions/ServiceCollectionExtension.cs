namespace CallMeSdk.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCallMeSdk(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddSingleton<IDataProviderFactory, DataProviderFactory>();
        
        services.AddTransient<IDataProvider<SoapConfiguration>, SoapDataProvider>();
        services.AddTransient<IDataProvider<RestConfiguration>, RestDataProvider>();
        services.AddTransient<IDataProvider<FtpConfiguration>, FtpDataProvider>();

        services.AddSingleton<ICustomerIdService, CustomerIdService>();
        services.AddTransient<ICustomerService, CustomerService>();

        services.AddTransient<ICompositeDataProvider, CompositeDataProvider>();
        services.AddTransient(typeof(ITimingDataProvider<>), typeof(TimingDataProvider<>));

        services.AddKeyedTransient<IFtpClient, FluentFtpClient>(FtpClientType.FluentFtp.GetKey());
        services.AddSingleton<IFtpClientConfigurator, FtpClientConfigurator>();
        services.AddSingleton<IFtpClientFactory, FtpClientFactory>();
        
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