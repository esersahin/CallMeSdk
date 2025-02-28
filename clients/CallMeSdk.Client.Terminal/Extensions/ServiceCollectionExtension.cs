namespace CallMeSdk.Client.Terminal.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResolvers().
            AddFactories().
            AddClientServices().
            AddStopWatchService().
            AddAzonBank(configuration).
            AddMikrozortBank(configuration).
            AddStrongLifeInsurance(configuration);
    }

    private static IServiceCollection AddResolvers(this IServiceCollection services)
    {
        services.AddSingleton<IConfigurationResolver, ConfigurationResolver>();

        return services;
    }

    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddSingleton<IDataAdapterFactory, DataAdapterFactory>();

        return services;
    }

    private static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerDataService, CustomerDataService>().
            AddSingleton<ICustomerProcessor, CustomerProcessor>();

        return services;
    }

    private static IServiceCollection AddStopWatchService(this IServiceCollection services)
    {
        services.AddTransient<IStopwatchService, DefaultStopwatchService>();
        return services;
    }


    private static IServiceCollection AddAzonBank(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<SoapConfiguration>(Clients.AzonBank, configuration).
            AddKeyedScoped<IDataAdapter, AzonBankSoapDataAdapter>(Clients.AzonBank).
            AddKeyedScoped<ICustomerProviderStrategy, AzonBankSoapStrategy>(Clients.AzonBank);

        return services;
    }

    private static IServiceCollection AddMikrozortBank(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<RestConfiguration>(Clients.MikrozortBank, configuration).
            AddKeyedScoped<IDataAdapter, MikrozortBankRestDataAdapter>(Clients.MikrozortBank).
            AddKeyedScoped<ICustomerProviderStrategy, MikrozortBankStrategy>(Clients.MikrozortBank);

        return services;
    }

    private static IServiceCollection AddStrongLifeInsurance(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<FtpConfiguration>(Clients.StrongLifeInsurance, configuration).
            AddKeyedScoped<IDataAdapter, StrongLifeInsuranceFtpDataAdapter>(Clients.StrongLifeInsurance).
            AddKeyedScoped<ICustomerProviderStrategy, StrongLifeInsuranceStrategy>(Clients.StrongLifeInsurance);

        return services;
    }
}