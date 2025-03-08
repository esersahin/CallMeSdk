using CallMeSdk.Serialization;

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
                 AddTransient<ICustomersService, CustomersService>();

        services.AddTransient<ICompositeDataProvider, CompositeDataProvider>().
                 AddTransient(typeof(ITimingDataProvider<>), typeof(TimingDataProvider<>));

        services.AddSingleton<IFtpClientConfigurator, FtpClientConfigurator>().
                 AddSingleton<IFtpClientFactory, FtpClientFactory>().
                 AddKeyedTransient<IFtpClient, FluentFtpClient>(FtpClientType.FluentFtp.GetKey());
        
        services.AddScoped<AppJsonSerializerContext>(provider =>
        {
            var customerIdService = provider.GetRequiredService<ICustomerIdService>();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = false,
                IncludeFields = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new CustomerIdJsonConverter(customerIdService));

            return new AppJsonSerializerContext(options);
        });
        
        return services;
    }
}