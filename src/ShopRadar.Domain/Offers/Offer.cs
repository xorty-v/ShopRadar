using ShopRadar.Domain.Abstractions;
using ShopRadar.Domain.PriceHistories;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Shared;
using ShopRadar.Domain.Stores;

namespace ShopRadar.Domain.Offers;

public sealed class Offer : Entity<OfferId>
{
    private readonly List<PriceHistory> _priceHistories = [];

    private Offer()
    {
    }

    private Offer(OfferId id, ProductId? productId, StoreId storeId, Name name, Url url) : base(id)
    {
        ProductId = productId;
        StoreId = storeId;
        Name = name;
        Url = url;
    }

    public ProductId? ProductId { get; private set; }
    public StoreId StoreId { get; private set; }
    public Name Name { get; private set; }
    public Url Url { get; private set; }
    public IReadOnlyList<PriceHistory> PriceHistories => _priceHistories;

    public static Result<Offer> Create(ProductId? productId, StoreId storeId, Name name, Url url)
    {
        var offer = new Offer(OfferId.New(), productId, storeId, name, url);

        return offer;
    }

    public static Result<Offer> Create(
        ProductId? productId,
        StoreId storeId,
        Name name,
        Url url,
        Money price,
        Money discountPrice,
        DateTime priceDateUtc)
    {
        var offer = new Offer(OfferId.New(), productId, storeId, name, url);

        var priceHistoryResult = PriceHistory.Create(offer.Id, price, discountPrice, priceDateUtc);
        if (priceHistoryResult.IsFailure)
        {
            return Result.Failure<Offer>(priceHistoryResult.Error);
        }

        offer._priceHistories.Add(priceHistoryResult.Value);

        return offer;
    }

    public Result UpdatePrice(Money newPrice, Money newDiscountPrice, DateTime lastPriceOnUtc)
    {
        var lastPrice = _priceHistories.LastOrDefault();

        if (lastPrice is not null &&
            lastPrice.Price == newPrice &&
            lastPrice.DiscountPrice == newDiscountPrice)
        {
            return Result.Success();
        }

        var newHistoryResult = PriceHistory.Create(Id, newPrice, newDiscountPrice, lastPriceOnUtc);

        if (newHistoryResult.IsFailure)
        {
            return Result.Failure(newHistoryResult.Error);
        }

        _priceHistories.Add(newHistoryResult.Value);

        return Result.Success();
    }
}