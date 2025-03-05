namespace CallMeSdk.Client.WebApi.Extensions;

public static class ServiceCollectionExtension
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
        services.AddScoped<IConfigurationResolver, ConfigurationResolver>().
                 AddScoped<IProviderResolver, AzonBankProviderResolver>().
                 AddScoped<IProviderResolver, MikrozortBankProviderResolver>().
                 AddScoped<IProviderResolver, StrongLifeInsuranceProviderResolver>();

        return services;
    }
    
    private static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddScoped<IDataAdapterFactory, DataAdapterFactory>().
                 AddScoped<ICustomerProviderFactory, CustomerProviderFactory>();
        
        return services;
    }

    private static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(CustomerProviderStrategy<>)).
                 AddScoped<ICustomerProviderService, CustomerProviderService>();
        
        return services;
    }
    
    private static IServiceCollection AddAzonBank(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<SoapConfiguration>(Clients.AzonBank, configuration).
                 AddKeyedScoped<IDataAdapter, SoapDataAdapter>(Clients.AzonBank);
     
        return services;
    }
    
    private static IServiceCollection AddMikrozortBank(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<RestConfiguration>(Clients.MikrozortBank, configuration).
                 AddKeyedScoped<IDataAdapter, RestDataAdapter>(Clients.MikrozortBank);

        return services;
    }

    private static IServiceCollection AddStrongLifeInsurance(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEndpoint<FtpConfiguration>(Clients.StrongLifeInsurance, configuration).
                 AddKeyedScoped<IDataAdapter, FtpDataAdapter>(Clients.StrongLifeInsurance);

        return services;
    }
}