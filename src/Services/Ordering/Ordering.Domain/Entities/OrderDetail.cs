using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domains;

namespace Ordering.Domain.Entities;

public class OrderDetail : EntityBase<Guid>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    public int ItemPrice { get; set; }
    public string? Thumbnail { get; set; }
    public virtual Order Order { get; set; } = null!;
}