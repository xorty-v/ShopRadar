using ShopRadar.Domain.Abstractions;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Shared;

namespace ShopRadar.Domain.Stores;

public sealed class Store : Entity<StoreId>
{
    private readonly List<Offer> _offers = [];

    private Store()
    {
    }

    private Store(StoreId id, string name, Url url) : base(id)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; private set; }
    public Url Url { get; private set; }
    public IReadOnlyList<Offer> Offers => _offers;

    public static Store Create(StoreId id, string name, Url url) => new(id, name, url);

    public Result AddOffer(Offer offer)
    {
        if (_offers.Any(o => o.Url == offer.Url))
        {
            return Result.Failure<Store>(StoreErrors.AlreadyExists);
        }

        _offers.Add(offer);
        return Result.Success();
    }
}