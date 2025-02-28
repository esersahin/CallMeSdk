namespace CallMeSdk.Extensions;

public static class ConfigurationExtension
{
    public static T GetConfiguration<T>(this IServiceScope scope, string clientName) where T : class
    {
        return scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<T>>().Get(clientName);
    }

    public static T GetConfiguration<T>(this IServiceProvider serviceProvider, string clientName) where T : class
    {
        return serviceProvider.GetRequiredService<IOptionsSnapshot<T>>().Get(clientName);
    }
    
    public static IServiceCollection ConfigureEndpoint<T>(this IServiceCollection services, 
        string client, 
        IConfiguration configuration) where T : class, IClientConfiguration
    {
        services.Configure<T>(client,configuration.GetSection($"AppConfigurations:Clients:{client}"));
     
        return services;
    }
}