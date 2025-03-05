# CallMeSdk - Çağrı merkezi simülasyonu için yazılmış bir yazılım geliştirme kitidir.

`CallMeSdk`, https://github.com/buraksenyurt/friday-night-programmer/blob/main/documents/UseCases.md adresinde ki use case UC00 senaryosuna uygun olarak geliştirilmiştir.  
REST, SOAP ve FTP gibi farklı protokoller üzerinden veri çekme işlemlerini kolaylaştıran bir SDK'dır.  
Bu SDK, esnek ve genişletilebilir bir yapı sunar.

## Özellikler

- **REST, SOAP ve FTP desteği**: Farklı protokoller üzerinden veri çekme işlemlerini destekler.
- **Esnek yapılandırma**: Yapılandırma bilgilerini dinamik olarak sağlayabilirsiniz.
- **Dependency Injection desteği**: .NET Core'un DI mekanizmasıyla tam uyumludur.
- **Genişletilebilirlik**: Yeni protokoller ve adaptasyon stratejileri eklenebilir.
- **.NET 9 ve C#:** En son .NET teknolojileri ile geliştirilmiştir.

## Mimari

SDK, aşağıdaki katmanlardan oluşuyor:

*   **Abstractions:** Arayüzler ve soyut sınıflar (örn. `IDataAdapter`, `IDataProvider`, `BaseDataProvider`).
*   **Configuration:** Yapılandırma sınıfları (örn. `FtpConfiguration`,`RestConfiguration`,`SoapConfiguration`).
*   **Converters:** Json verisini ValueObject'e dönüştüren çeviriciler (örn. `CustomerIdJsonConverter`).
*   **DataProviders:** Veri sağlayıcı sınıfları (örn. `FtpDataProvider`, `RestDataProvider`, `SoapDataProvider`).
*   **Domain Services:** Domain Servisleri (örn. `CustomerIdService`).
*   **Extensions:** Extension Sınıfları (örn. `ServiceCollectionExtensions`). 
*   **FtpClients:** Ftp istemcileri, konfigürator ve fabrika sınıfları (örn. `FluentFtpClient, FtpClientConfigurator, FtpClientFactory`).  
*   **Models:** Veri modelleri (örn. `Customer`).
*   **Services:** Servis sınıfları (örn. `CustomerService`).
*   **Value Objects:** Domain modelleri için değer nesneleri (örn. `CustomerId`).

## Kullanım

1.  Web API 

    ```csharp
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCallMeSdk()
    
    var app = builder.Build();
    ```

2.  Console 

    ```csharp
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddCallMeSdk();
    
    var serviceProvider = serviceCollection.BuildServiceProvider();
    ```

## Örnekler

1. 
    ```csharp
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerProviderService _customerProviderService;

        public CustomersController(ICustomerProviderService customerProviderService)
        {
            _customerProviderService = customerProviderService;
        }

        [HttpGet("{providerType}")]
        public async Task<IActionResult> GetCustomers(string providerType)
        {
            var customers = await _customerProviderService.GetCustomersAsync(providerType);
            return customers is not null ? Ok(customers) : NotFound("Provider not found");
        }
    }
    ```
    Web API istekleri  
    http://localhost:5000/api/customers/AzonBank  
    http://localhost:5000/api/customers/MikrozortBank  
    http://localhost:5000/api/customers/StrongLifeInsurance
2.
     ```csharp
    using (var scope = serviceProvider.CreateScope())
    {
        var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
        var adapterFactory = scope.ServiceProvider.GetRequiredService<IDataAdapterFactory>();

        var soapConfiguration = scope.GetConfiguration<SoapConfiguration>(Clients.AzonBank);
        var soapAdapter = adapterFactory.Create(ClientProviderType.AzonBank);
        var soapCustomers = await customerService.GetCustomersAsync(soapConfiguration, soapAdapter);
        soapCustomers.PrintCustomers(Clients.AzonBank);
    
        var restConfiguration = scope.GetConfiguration<RestConfiguration>(Clients.MikrozortBank);
        var restAdapter = adapterFactory.Create(ClientProviderType.MikrozortBank);
        var restCustomers = await customerService.GetCustomersAsync(restConfiguration, restAdapter);
        restCustomers.PrintCustomers(Clients.MikrozortBank);
    
        var ftpConfiguration = scope.GetConfiguration<FtpConfiguration>(Clients.StrongLifeInsurance);
        var ftpAdapter = adapterFactory.Create(ClientProviderType.StrongLifeInsurance);
        var ftpCustomers = await customerService.GetCustomersAsync(ftpConfiguration, ftpAdapter);
        ftpCustomers.PrintCustomers(Clients.StrongLifeInsurance);
    }
    ```
3.
     ```csharp
    using (var scope = serviceProvider.CreateScope())
    {
        var customerDataService = scope.ServiceProvider.GetRequiredService<ICustomerDataService>();
        await customerDataService.RetrieveAndPrintCustomersAsync();
    }
    ```
4.
     ```csharp
    using (var scope = serviceProvider.CreateScope())
    {
        var processor = scope.ServiceProvider.GetRequiredService<ICustomerProcessor>();
        await processor.ProcessCustomersAsync();
    }
     ```
5. 
     ```csharp
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

          var restDataAdapterStrategy = dataAdapterFactory.Create(ClientProviderType.MikrozortBank);
          var soapDataAdapterStrategy = dataAdapterFactory.Create(ClientProviderType.AzonBank);    
          var ftpDataAdapterStrategy = dataAdapterFactory.Create(ClientProviderType.StrongLifeInsurance);
    
          compositeProvider.AddProvider(restProvider, restConfiguration, restDataAdapterStrategy);
          compositeProvider.AddProvider(soapProvider, soapConfiguration, soapDataAdapterStrategy);
          compositeProvider.AddProvider(ftpProvider, ftpConfiguration, ftpDataAdapterStrategy);

          var customers = await compositeProvider.FetchAsync();

          customers.PrintCustomers("Composite Provider");
      }
     ```
   
6.  
     ```csharp
      using (var scope = serviceProvider.CreateScope())
      {
          var soapConfiguration = scope.GetConfiguration<SoapConfiguration>(Clients.AzonBank);
          var restConfiguration = scope.GetConfiguration<RestConfiguration>(Clients.MikrozortBank);
          var ftpConfiguration = scope.GetConfiguration<FtpConfiguration>(Clients.StrongLifeInsurance);

          var dataAdapterFactory = scope.ServiceProvider.GetRequiredService<IDataAdapterFactory>();

          var soapDataAdapterStrategy = dataAdapterFactory.Create(ClientProviderType.AzonBank);
          var restDataAdapterStrategy = dataAdapterFactory.Create(ClientProviderType.MikrozortBank);
          var ftpDataAdapterStrategy = dataAdapterFactory.Create(ClientProviderType.StrongLifeInsurance);

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
     ```
       

       
## Yapılandırma

SDK'nın davranışı, `appsettings.json` aracılığıyla yapılandırılabilir.

*   **Endpoint'ler:** Endpoint bilgileri (`BaseUrl`, `Endpoint`, `ClientType`, vb.) yapılandırma dosyalarında belirtilebilir.

```json
{
  "AppConfigurations" :
  {
    "Clients": {
      "AzonBank": {
        "BaseUrl": "http://localhost:8080",
        "Endpoint": "/api/customers/soap"
      },
      "MikrozortBank": {
        "BaseUrl": "http://localhost:8080",
        "Endpoint": "/api/customers/rest"
      },
      "StrongLifeInsurance": {
        "FtpServer": "localhost",
        "FilePath": "/customers/customers.csv",
        "Username": "myuser",
        "Password": "mypass",
        "ClientType": "FluentFtp"
      }
    }
  }
}
```

## Genişletilebilirlik

*   **Yeni DataProvider'lar:** `IDataProvider` arayüzünü uygulayan yeni sınıflar oluşturularak varolan `SoapDataProvider`,`RestDataProvider`,`FtpDataProvider` gibi yeni stratejiler eklenebilir.
*   **Yeni DataAdapter'lar :** `IDataAdapter` arayüzünü uygulayan yeni sınıflar olusturularak CallMeSdk.Client.DataAdapters projesinde varolan  `SoapDataAdapter`,`RestDataAdapter`,`FtpDataAdapter` gibi yeni adaptörler eklenebilir.

## Geliştirme ortamı için alt yapı
*  SOAP ve REST istekleri icin wiremock docker imaji ve kurulumu
    ```bash
    docker pull wiremock/wiremock
    ```

    ```bash
    docker run -d -p 8080:8080 wiremock/wiremock
    ```
* Wiremock endpoint bilgileri

#### soap-endpoint.json

```json
{
  "request": {
    "method": "POST",
    "url": "/api/customers/soap",
    "headers": {
      "Content-Type": {
        "contains": "text/xml"
      }
    },
    "bodyPatterns": [
      {
        "contains": "<SOAP-ENV:Envelope"
      }
    ]
  },
  "response": {
    "status": 200,
    "headers": {
      "Content-Type": "text/xml"
    },
    "body": "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\">\n  <SOAP-ENV:Body>\n    <Customers>\n      <Customer>\n        <CustomerId>CES12345</CustomerId>\n        <Name>Michael Scott</Name>\n        <Email>mscott@ces.com</Email>\n      </Customer>\n      <Customer>\n        <CustomerId>CES56789</CustomerId>\n        <Name>Pam Beesly</Name>\n        <Email>pbeesly@ces.com</Email>\n      </Customer>\n    </Customers>\n  </SOAP-ENV:Body>\n</SOAP-ENV:Envelope>"
  }
}
```

#### json-endpoint.json

```json
{
  "request": {
    "method": "GET",
    "url": "/api/customers/rest"
  },
  "response": {
    "status": 200,
    "headers": {
      "Content-Type": "application/json"
    },
    "jsonBody": [
      {
        "CustomerId": "ABC12345",
        "Name": "John Doe",
        "Email": "johndoe@abc.com"
      },
      {
        "CustomerId": "DEF12345",
        "Name": "Jane Doe",
        "Email": "janedoe@abc.com"
      }
    ]
  }
}
```

* Wiremock api'sini kullanarak mapping işlemleri  

    ```bash
    curl -X POST http://localhost:8080/__admin/mappings \
     -H "Content-Type: application/json" \
     -d @soap-endpoint.json
    ```

    ```bash
    curl -X POST http://localhost:8080/__admin/mappings \
     -H "Content-Type: application/json" \
     -d @json-endpoint.json     
    ```
* Docker'ı yeniden başlattığınızda yukarıda ki komutları çalıştırmalısınız.  
  Bu tekrarlı işlemin yerine WireMock'un Docker konteyneri içerisine SOAP ve REST config dosyalarını  
  otomatik olarak yüklemek için, yerel makinedeki dosyaları WireMock'un /home/wiremock/mappings dizinine bağlamanız gerekir.
    ```bash
    docker run --rm -p 8080:8080 \
    -v $(pwd)/wiremock/mappings:/home/wiremock/mappings \
    wiremock/wiremock
    ```
  Açıklamalar:
    * --rm: Konteyner kapandığında otomatik olarak temizlenmesini sağlar.  
    * -p 8080:8080: WireMock'u 8080 portunda çalıştırır.  
    * -v $(pwd)/wiremock/mappings:/home/wiremock/mappings:  
    * Yerel klasör: $(pwd)/wiremock/mappings  
    WireMock içindeki hedef klasör: /home/wiremock/mappings  
    Bu eşleştirme, yerel makinedeki wiremock/mappings klasöründeki dosyaların WireMock konteynerine otomatik olarak yüklenmesini sağlar.


* Wiremock endpoint testleri
    ```bash
    curl -X POST http://localhost:8080/api/customers/soap 
         -H "Content-Type: text/xml"  
         -d "<?xml version='1.0'?><SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'><SOAP-ENV:Body></SOAP-ENV:Body></SOAP-ENV:Envelope>"    
    ```
  
    ```bash
    curl -X GET http://localhost:8080/api/customers/rest    
    ```
* Ftp istekleri icin fauria/vsftpd docker imaji ve kurulumu.

    ```bash
    docker pull fauria/vsftpd
    ```

    ```bash
    docker run -d \
      -v /path/to/local/folder:/home/vsftpd \
      -e FTP_USER=myuser \
      -e FTP_PASS=mypass \
      -e PASV_ADDRESS=127.0.0.1 
      -e PASV_MIN_PORT=21100 
      -e PASV_MAX_PORT=21110 \
      -p 20:20 -p 21:21 -p 21100-21110:21100-21110 \
      --name vsftpd --restart=always fauria/vsftpd
    ```
* customers.csv dosya icerigi  
  CUST0001,Harper Lee,hlee@tkam.com  
  CUST0002,George Orwell,go1984@gorwell.com
## Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## İletişim

[eser.sahin@gmail.com]