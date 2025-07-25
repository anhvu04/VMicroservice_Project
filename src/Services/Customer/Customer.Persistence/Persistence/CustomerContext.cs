using Customer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer.Persistence.Persistence;

public class CustomerContext : DbContext
{
    public DbSet<CustomerSegment> CustomerSegments { get; set; }

    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CustomerSegment>().HasIndex(x => x.Email).IsUnique();
    }
}