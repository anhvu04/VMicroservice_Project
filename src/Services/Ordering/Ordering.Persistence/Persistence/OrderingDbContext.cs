using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Persistence.Persistence;

public class OrderingDbContext : DbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
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
    }
}