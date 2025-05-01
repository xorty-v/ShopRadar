using ShopRadar.Application.Abstractions.Parsers;
using ShopRadar.Domain.Offers;
using ShopRadar.Parsers.Abstractions;

namespace ShopRadar.Parsers.Alta;

public class AltaParser : BaseParser, IParser
{
    protected override string CategoryUrl { get; }
    protected override string ProductUrl { get; }

    public Task<List<Offer>> ParseAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<List<CategoryRaw>> ParseCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<List<Offer>> ParseOffersAsync(List<CategoryRaw> categories)
    {
        throw new NotImplementedException();
    }
}