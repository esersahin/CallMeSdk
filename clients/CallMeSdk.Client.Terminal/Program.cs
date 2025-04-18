﻿using CallMeSdk.Serialization;

var serviceCollection = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

serviceCollection.
    AddCallMeSdk().
    AddClients(configuration);

var serviceProvider = serviceCollection.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddConfiguration(configuration.GetSection("Logging"));
}).BuildServiceProvider();

// Data retrieval approaches

// Approach I
using (var scope = serviceProvider.CreateScope())
{
    var customersService = scope.ServiceProvider.GetRequiredService<ICustomersService>();
    var jsonSerializerContext = scope.ServiceProvider.GetRequiredService<AppJsonSerializerContext>();
    var customerIdService = scope.ServiceProvider.GetRequiredService<ICustomerIdService>();
    
    var soapConfiguration = scope.GetConfiguration<SoapConfiguration>(Clients.AzonBank);
    var soapAdapter = new SoapDataAdapter(customerIdService);
    var soapCustomers = await customersService.GetCustomersAsync(soapConfiguration, soapAdapter);
    soapCustomers.PrintCustomers(Clients.AzonBank);
    
    var restConfiguration = scope.GetConfiguration<RestConfiguration>(Clients.MikrozortBank);
    var restAdapter = new RestDataAdapter(jsonSerializerContext);
    var restCustomers = await customersService.GetCustomersAsync(restConfiguration, restAdapter);
    restCustomers.PrintCustomers(Clients.MikrozortBank);
    
    var ftpConfiguration = scope.GetConfiguration<FtpConfiguration>(Clients.StrongLifeInsurance);
    var ftpAdapter = new FtpDataAdapter(customerIdService);
    var ftpCustomers = await customersService.GetCustomersAsync(ftpConfiguration, ftpAdapter);
    ftpCustomers.PrintCustomers(Clients.StrongLifeInsurance);
}

// Approach II
using (var scope = serviceProvider.CreateScope())
{
    var customerService = scope.ServiceProvider.GetRequiredService<ICustomersService>();
    var dataAdapterFactory = scope.ServiceProvider.GetRequiredService<IDataAdapterFactory>();
   
    var soapConfiguration = scope.GetConfiguration<SoapConfiguration>(Clients.AzonBank);
    var soapAdapter = dataAdapterFactory.Create(Clients.AzonBank);
    var soapCustomers = await customerService.GetCustomersAsync(soapConfiguration, soapAdapter);
    soapCustomers.PrintCustomers(Clients.AzonBank);
    
    var restConfiguration = scope.GetConfiguration<RestConfiguration>(Clients.MikrozortBank);
    var restAdapter = dataAdapterFactory.Create(Clients.MikrozortBank);
    var restCustomers = await customerService.GetCustomersAsync(restConfiguration, restAdapter);
    restCustomers.PrintCustomers(Clients.MikrozortBank);
    
    var ftpConfiguration = scope.GetConfiguration<FtpConfiguration>(Clients.StrongLifeInsurance);
    var ftpAdapter = dataAdapterFactory.Create(Clients.StrongLifeInsurance);
    var ftpCustomers = await customerService.GetCustomersAsync(ftpConfiguration, ftpAdapter);
    ftpCustomers.PrintCustomers(Clients.StrongLifeInsurance);
}

// Approach III
using (var scope = serviceProvider.CreateScope())
{
    var customersDataService = scope.ServiceProvider.GetRequiredService<ICustomersDataService>();
    await customersDataService.RetrieveAndPrintCustomersAsync();
}

// Approach IV
using (var scope = serviceProvider.CreateScope())
{
    var processor = scope.ServiceProvider.GetRequiredService<ICustomersProcessor>();
    await processor.ProcessCustomersAsync();
}

// Approach V
using (var scope = serviceProvider.CreateScope())
{
    var dataProviderFactory = scope.ServiceProvider.GetRequiredService<IDataProviderFactory>();
    var dataAdapterFactory = scope.ServiceProvider.GetRequiredService<IDataAdapterFactory>();
    var compositeProvider = scope.ServiceProvider.GetRequiredService<ICompositeDataProvider>();

    var soapConfiguration = scope.GetConfiguration<SoapConfiguration>(Clients.AzonBank);
    var restConfiguration = scope.GetConfiguration<RestConfiguration>(Clients.MikrozortBank);
    var ftpConfiguration = scope.GetConfiguration<FtpConfiguration>(Clients.StrongLifeInsurance);
    
    var soapProvider = dataProviderFactory.Create<SoapConfiguration>();
    var restProvider = dataProviderFactory.Create<RestConfiguration>();
    var ftpProvider = dataProviderFactory.Create<FtpConfiguration>();

    var restDataAdapterStrategy = dataAdapterFactory.Create(Clients.MikrozortBank);
    var soapDataAdapterStrategy = dataAdapterFactory.Create(Clients.AzonBank);    
    var ftpDataAdapterStrategy = dataAdapterFactory.Create(Clients.StrongLifeInsurance);
    
    compositeProvider.AddProvider(restProvider, restConfiguration, restDataAdapterStrategy);
    compositeProvider.AddProvider(soapProvider, soapConfiguration, soapDataAdapterStrategy);
    compositeProvider.AddProvider(ftpProvider, ftpConfiguration, ftpDataAdapterStrategy);

    var customers = await compositeProvider.FetchAsync();

    customers.PrintCustomers("Composite Provider");
}

// Approach VI
using (var scope = serviceProvider.CreateScope())
{
    var soapConfiguration = scope.GetConfiguration<SoapConfiguration>(Clients.AzonBank);
    var restConfiguration = scope.GetConfiguration<RestConfiguration>(Clients.MikrozortBank);
    var ftpConfiguration = scope.GetConfiguration<FtpConfiguration>(Clients.StrongLifeInsurance);

    var dataAdapterFactory = scope.ServiceProvider.GetRequiredService<IDataAdapterFactory>();

    var soapDataAdapterStrategy = dataAdapterFactory.Create(Clients.AzonBank);
    var restDataAdapterStrategy = dataAdapterFactory.Create(Clients.MikrozortBank);
    var ftpDataAdapterStrategy = dataAdapterFactory.Create(Clients.StrongLifeInsurance);

    var soapTimingDataProvider = scope.ServiceProvider.GetRequiredService<ITimingDataProvider<SoapConfiguration>>();
    soapTimingDataProvider.Configure(soapConfiguration);
    soapTimingDataProvider.SetCorporateName(Clients.AzonBank);
    var soapCustomers = await soapTimingDataProvider.FetchAsync(soapDataAdapterStrategy);
    soapCustomers.PrintCustomers($"{Clients.AzonBank} Timing Data Provider");

    var restTimingDataProvider = scope.ServiceProvider.GetRequiredService<ITimingDataProvider<RestConfiguration>>();
    restTimingDataProvider.Configure(restConfiguration);
    restTimingDataProvider.SetCorporateName(Clients.MikrozortBank);
    var restCustomers = await restTimingDataProvider.FetchAsync(restDataAdapterStrategy);
    restCustomers.PrintCustomers($"{Clients.MikrozortBank} Timing Data Provider");

    var ftpTimingDataProvider = scope.ServiceProvider.GetRequiredService<ITimingDataProvider<FtpConfiguration>>();
    ftpTimingDataProvider.Configure(ftpConfiguration);
    ftpTimingDataProvider.SetCorporateName(Clients.StrongLifeInsurance);
    var ftpCustomers = await ftpTimingDataProvider.FetchAsync(ftpDataAdapterStrategy);
    ftpCustomers.PrintCustomers($"{Clients.StrongLifeInsurance} Timing Data Provider");
}