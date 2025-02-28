namespace CallMeSdk.ValueObjects;

public sealed record CustomerId
{
    public string? Value { get; }

    private CustomerId(string? value)
    {
        Value = value;
    }

    // Factory method for creating a valid CustomerId
    public static CustomerId Create(string? value, ICustomerIdService customerIdService)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CustomerId cannot be null or empty.");

        if (!customerIdService.IsValidCustomerId(value))
            throw new ArgumentException("Invalid CustomerId format.");

        return new CustomerId(value);
    }

    public override string? ToString() => Value; 
}