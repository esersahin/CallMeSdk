namespace CallMeSdk.Models;

public class Customer
{
    public required CustomerId CustomerId { get; init; }
    public required string? Name { get; init; }
    public string? Email { get; init; }
}