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
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}