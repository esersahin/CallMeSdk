namespace CallMeSdk.Client.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResolvers().
            AddFactories().
            AddClientServices().
            AddAzonBank(configuration).
            AddMikrozortBank(configuration).
            AddStrongLifeInsurance(configuration);
    }
    
    private static IServiceCollection AddResolvers(this IServiceCollection services)
    {
        services.AddScoped<IConfigurationResolver, ConfigurationResolver>();

        services.AddScoped<IProviderResolver, AzonBankProviderResolver>();
        services.AddScoped<IProviderResolver, MikrozortBankProviderResolver>();
        services.AddScoped<IProviderResolver, StrongLifeInsuranceProviderResolver>();

        return services;
    }
    
    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddScoped<IDataAdapterFactory, DataAdapterFactory>();
        services.AddScoped<ICustomerProviderFactory, CustomerProviderFactory>();
        
        return services;
    }

    private static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddScoped<CustomerProviderStrategy<SoapConfiguration>>();
        services.AddScoped<CustomerProviderStrategy<RestConfiguration>>();
        services.AddScoped<CustomerProviderStrategy<FtpConfiguration>>();

        services.AddScoped(typeof(CustomerProviderStrategy<>));
        
        services.AddScoped<ICustomerProviderService, CustomerProviderService>();
        
        return services;
    }
    
    private static IServiceCollection AddAzonBank(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<SoapConfiguration>(Clients.AzonBank, configuration).
            AddKeyedScoped<IDataAdapter, AzonBankSoapDataAdapter>(Clients.AzonBank);
     
        return services;
    }
    
    private static IServiceCollection AddMikrozortBank(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<RestConfiguration>(Clients.MikrozortBank, configuration).
            AddKeyedScoped<IDataAdapter, MikrozortBankRestDataAdapter>(Clients.MikrozortBank);

        return services;
    }

    private static IServiceCollection AddStrongLifeInsurance(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<FtpConfiguration>(Clients.StrongLifeInsurance, configuration).
            AddKeyedScoped<IDataAdapter, StrongLifeInsuranceFtpDataAdapter>(Clients.StrongLifeInsurance);

        return services;
    }
}