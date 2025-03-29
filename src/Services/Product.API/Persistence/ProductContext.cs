using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Persistence;

public class ProductContext : DbContext
{
    public DbSet<CatalogProduct> Products { get; set; } = null!;

    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }
}