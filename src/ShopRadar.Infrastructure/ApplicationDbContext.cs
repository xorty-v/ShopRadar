using Microsoft.EntityFrameworkCore;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Stores;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}