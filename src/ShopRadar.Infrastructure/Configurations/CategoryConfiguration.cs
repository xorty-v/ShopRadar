using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        builder.Property(c => c.Url).IsRequired().HasMaxLength(100);

        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);

        builder.HasData(new Category
            {
                Id = Guid.NewGuid(),
                StoreId = Guid.Parse("bf167e4c-799a-4bdd-aa41-75ebabe9089b"),
                Name = "Test Category",
                Url = "TEST"
            }
        );
    }
}