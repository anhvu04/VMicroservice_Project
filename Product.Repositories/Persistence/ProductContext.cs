using Microsoft.EntityFrameworkCore;
using Product.Repositories.Entities;

namespace Product.Repositories.Persistence;

public class ProductContext : DbContext
{
    public DbSet<CatalogProduct> Products { get; set; } = null!;

    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }
}