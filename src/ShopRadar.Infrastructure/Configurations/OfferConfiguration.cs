using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Stores;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(id => id.Value, value => OfferId.Create(value))
            .IsRequired();

        builder.Property(o => o.StoreId)
            .HasConversion(id => id.Value, value => StoreId.Create(value))
            .IsRequired();

        builder.Property(o => o.ProductId)
            .HasConversion(id => id!.Value, value => ProductId.Create(value))
            .IsRequired(false);

        builder.OwnsOne(o => o.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired();
        });

        builder.OwnsOne(o => o.Url, url =>
        {
            url.Property(u => u.Value)
                .HasColumnName("Url")
                .IsRequired();
        });

        builder.HasMany(o => o.PriceHistories)
            .WithOne()
            .HasForeignKey(ph => ph.OfferId)
            .IsRequired();
    }
}