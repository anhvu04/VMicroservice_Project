namespace Customer.Services.Models.Requests.CustomerSegment;

public class UpdateCustomerSegmentRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
}