namespace Contracts.Domains.Interfaces;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
}