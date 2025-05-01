using ShopRadar.Domain.Shared;

namespace ShopRadar.Domain.Offers;

public interface IOfferRepository
{
    public Task AddOffersAsync(List<Offer> offers, CancellationToken cancellationToken = default);
    public Task<Offer?> GetOfferByUrlAsync(Url url, CancellationToken cancellationToken);
}