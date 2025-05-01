using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => ProductId.Create(value))
            .IsRequired();

        builder.Property(p => p.CategoryId)
            .HasConversion(id => id.Value, value => CategoryId.Create(value))
            .IsRequired();

        builder.OwnsOne(p => p.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired();
        });

        builder.HasMany(p => p.Offers)
            .WithOne()
            .HasForeignKey(o => o.ProductId)
            .IsRequired(false);
    }
}