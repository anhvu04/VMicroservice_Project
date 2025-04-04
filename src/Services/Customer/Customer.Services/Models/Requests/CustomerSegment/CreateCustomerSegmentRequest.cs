using Infrastructure.Utils;

namespace Customer.Services.Models.Requests.CustomerSegment;

public class CreateCustomerSegmentRequest : ValidatableObject
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public string? ImageUrl { get; set; }
}