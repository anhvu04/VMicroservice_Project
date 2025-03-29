namespace Contracts.Domains.Interfaces;

public interface IEntityAuditBase<TKey> : IEntityBase<TKey>, IAuditable
{
}