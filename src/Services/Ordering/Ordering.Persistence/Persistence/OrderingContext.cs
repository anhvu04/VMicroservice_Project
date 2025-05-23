using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Persistence.Persistence;

public class OrderingContext : DbContext
{
    public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // Order
        modelBuilder.Entity<Order>().Property(x => x.OrderStatus).HasConversion(x => x.ToString(),
            x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));
        modelBuilder.Entity<Order>().Property(x => x.PaymentMethod).HasConversion(x => x.ToString(),
            x => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), x));
    }
}