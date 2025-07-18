using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;

namespace Product.Persistence.Persistence;

public class ProductContext : DbContext
{
    public DbSet<CatalogProduct> Products { get; set; } = null!;

    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }
}