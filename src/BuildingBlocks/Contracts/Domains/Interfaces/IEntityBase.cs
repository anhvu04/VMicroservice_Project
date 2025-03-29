using System.ComponentModel.DataAnnotations;

namespace Contracts.Domains.Interfaces;

public interface IEntityBase<TKey>
{
    [Key] public TKey Id { get; set; }
}