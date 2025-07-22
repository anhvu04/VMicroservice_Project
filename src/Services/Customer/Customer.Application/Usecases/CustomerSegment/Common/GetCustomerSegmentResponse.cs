namespace Customer.Application.Usecases.CustomerSegment.Common;

public class GetCustomerSegmentResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? ImageUrl { get; set; }
}