namespace CallMeSdk.Client.WebApi.Configurations;

public interface IConfigurationResolver
{
    TConfiguration GetConfiguration<TConfiguration>(string providerName)
        where TConfiguration : class, IClientConfiguration;
}

public class ConfigurationResolver
(
    IServiceProvider serviceProvider
) : IConfigurationResolver
{
    public TConfig GetConfiguration<TConfig>(string providerName) where TConfig : class, IClientConfiguration
    {
        var config = serviceProvider.GetConfiguration<TConfig>(providerName);

        return config ?? throw new InvalidOperationException($"Configuration retrieval failed for {providerName}.");
    }
}
