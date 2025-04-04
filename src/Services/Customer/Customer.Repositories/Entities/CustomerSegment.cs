using System.Linq.Expressions;
using Contracts.Domains;

namespace Customer.Repositories.Entities;

public class CustomerSegment : EntityAuditBase<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public string? ImageUrl { get; set; }

    public static Expression<Func<CustomerSegment, object>> GetSortValue(string sortBy)
    {
        return sortBy switch
        {
            "email" => x => x.Email,
            "userName" => x => x.UserName,
            _ => x => x.Id
        };
    }
}