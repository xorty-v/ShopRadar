using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain.Stores;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(35);

        builder.Property(s => s.Url).IsRequired().HasMaxLength(50);

        builder.HasMany(s => s.Categories)
            .WithOne(c => c.Store)
            .HasForeignKey(c => c.StoreId);

        builder.HasData(new Store
            {
                Id = Guid.Parse("bf167e4c-799a-4bdd-aa41-75ebabe9089b"),
                Name = "EliteElectronics",
                Url = "https://ee.ge/"
            }
        );
    }
}