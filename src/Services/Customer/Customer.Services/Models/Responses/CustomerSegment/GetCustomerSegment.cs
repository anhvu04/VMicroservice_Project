namespace Customer.Services.Models.Responses.CustomerSegment;

public class GetCustomerSegment
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
}