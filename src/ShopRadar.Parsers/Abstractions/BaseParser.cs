using ShopRadar.Domain.Offers;

namespace ShopRadar.Parsers.Abstractions;

public abstract class BaseParser
{
    protected abstract string CategoryUrl { get; }
    protected abstract string ProductUrl { get; }

    public abstract Task<List<CategoryRaw>> ParseCategoriesAsync();
    public abstract Task<List<Offer>> ParseOffersAsync(List<CategoryRaw> categories);
}