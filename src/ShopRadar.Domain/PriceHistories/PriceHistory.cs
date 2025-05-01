using ShopRadar.Domain.Abstractions;
using ShopRadar.Domain.Offers;

namespace ShopRadar.Domain.PriceHistories;

public sealed class PriceHistory : Entity<PriceHistoryId>
{
    private PriceHistory()
    {
    }

    private PriceHistory(
        PriceHistoryId id,
        OfferId offerId,
        Money price,
        Money discountPrice,
        DateTime lastPriceOnUtc)
        : base(id)
    {
        OfferId = offerId;
        Price = price;
        DiscountPrice = discountPrice;
        LastPriceOnUtc = lastPriceOnUtc;
    }

    public OfferId OfferId { get; private set; }
    public Money Price { get; private set; }
    public Money DiscountPrice { get; private set; }
    public DateTime LastPriceOnUtc { get; private set; }

    public static Result<PriceHistory> Create(
        OfferId offerId,
        Money price,
        Money discountPrice,
        DateTime lastPriceOnUtc)
    {
        if (discountPrice > price)
        {
            return Result.Failure<PriceHistory>(PriceHistoryErrors.DiscountExceedsBasePrice);
        }

        var priceHistory = new PriceHistory(PriceHistoryId.New(), offerId, price, discountPrice, lastPriceOnUtc);

        return priceHistory;
    }
}