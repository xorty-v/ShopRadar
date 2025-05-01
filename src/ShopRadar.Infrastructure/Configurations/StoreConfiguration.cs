using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain;
using ShopRadar.Domain.Stores;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, value => StoreId.Create(value))
            .IsRequired();

        builder.Property(s => s.Name)
            .IsRequired();

        builder.OwnsOne(s => s.Url, url =>
        {
            url.Property(u => u.Value)
                .HasColumnName("Url")
                .IsRequired();
        });

        builder.HasMany(s => s.Offers)
            .WithOne()
            .HasForeignKey(o => o.StoreId)
            .IsRequired();

        builder.HasData(
            new
            {
                Id = Constants.PredefinedIds.Stores.Alta,
                Name = "Alta"
            },
            new
            {
                Id = Constants.PredefinedIds.Stores.Zoommer,
                Name = "Zoommer"
            },
            new
            {
                Id = Constants.PredefinedIds.Stores.EliteElectronic,
                Name = "EliteElectronic"
            }
        );

        builder.OwnsOne(s => s.Url).HasData(
            new
            {
                StoreId = Constants.PredefinedIds.Stores.Alta,
                Value = "https://alta.ge/?sl=en"
            },
            new
            {
                StoreId = Constants.PredefinedIds.Stores.Zoommer,
                Value = "https://zoommer.ge/en"
            },
            new
            {
                StoreId = Constants.PredefinedIds.Stores.EliteElectronic,
                Value = "https://ee.ge/"
            }
        );
    }
}