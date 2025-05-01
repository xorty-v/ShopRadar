using Microsoft.EntityFrameworkCore;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Shared;

namespace ShopRadar.Infrastructure.Repositories;

internal sealed class OfferRepository : IOfferRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OfferRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddOffersAsync(List<Offer> offers, CancellationToken cancellationToken)
    {
        _dbContext.Offers.AddRange(offers);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Offer?> GetOfferByUrlAsync(Url url, CancellationToken cancellationToken)
    {
        return await _dbContext.Offers.FirstOrDefaultAsync(o => o.Url == url, cancellationToken);
    }
}