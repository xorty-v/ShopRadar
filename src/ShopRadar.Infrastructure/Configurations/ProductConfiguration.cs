using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain.Products;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        builder.Property(p => p.DiscountPrice).HasColumnType("decimal(18,2)");

        builder.Property(p => p.Url).IsRequired().HasMaxLength(350);
    }
}