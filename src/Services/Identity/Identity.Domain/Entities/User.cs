using Contracts.Domains;
using Contracts.Domains.Entity;
using Shared.Enums;

namespace Identity.Domain.Entities;

public class User : EntityAuditBase<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public UserRoles Role { get; set; }
}