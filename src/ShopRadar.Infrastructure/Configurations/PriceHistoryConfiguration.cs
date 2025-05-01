using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.PriceHistories;

namespace ShopRadar.Infrastructure.Configurations;

internal sealed class PriceHistoryConfiguration : IEntityTypeConfiguration<PriceHistory>
{
    public void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.Id)
            .HasConversion(id => id.Value, value => PriceHistoryId.Create(value))
            .IsRequired();

        builder.Property(ph => ph.OfferId)
            .HasConversion(id => id.Value, value => OfferId.Create(value))
            .IsRequired();

        builder.OwnsOne(ph => ph.Price, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("Price")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.OwnsOne(ph => ph.DiscountPrice, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("DiscountPrice")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.Property(ph => ph.LastPriceOnUtc)
            .IsRequired();
    }
}