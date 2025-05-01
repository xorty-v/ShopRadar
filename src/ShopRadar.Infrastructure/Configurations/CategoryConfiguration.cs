using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => CategoryId.Create(value))
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired();

        builder.HasMany(c => c.Products)
            .WithOne()
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();

        builder.HasData(
            Category.Create(Constants.PredefinedIds.Categories.Laptops, "Laptops"),
            Category.Create(Constants.PredefinedIds.Categories.Smartphones, "Smartphones"),
            Category.Create(Constants.PredefinedIds.Categories.Monitors, "Monitors"),
            Category.Create(Constants.PredefinedIds.Categories.Headphones, "Headphones"),
            Category.Create(Constants.PredefinedIds.Categories.Keyboards, "Keyboards")
        );
    }
}