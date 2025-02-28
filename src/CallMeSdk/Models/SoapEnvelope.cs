namespace CallMeSdk.Models;

[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public sealed class SoapEnvelope
{
    [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public SoapBody? SoapBody { get; set; }
}

public sealed class SoapBody
{
    [XmlElement(ElementName = "Customers", Namespace = "")]
    public SoapCustomers? SoapCustomers { get; set; }
}

[XmlRoot(ElementName = "Customer", Namespace = "")]
public sealed class SoapCustomerDto
{
    [XmlElement(ElementName = "CustomerId")]
    public string? CustomerId { get; set; }

    [XmlElement(ElementName = "Name")]
    public string? Name { get; set; }

    [XmlElement(ElementName = "Email")]
    public string? Email { get; set; }
}

[XmlRoot(ElementName = "Customers")]
public sealed class SoapCustomers
{
    [XmlElement(ElementName = "Customer")]
    public List<SoapCustomerDto>? CustomerList { get; set; }
}