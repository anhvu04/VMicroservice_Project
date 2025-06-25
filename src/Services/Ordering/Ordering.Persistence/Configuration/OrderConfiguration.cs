using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Persistence.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.OrderStatus).HasConversion(o => o.ToString(),
            o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));
        builder.Property(x => x.PaymentMethod).HasConversion(o => o.ToString(),
            o => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), o));
    }
}