using ShopRadar.Domain.Offers;

namespace ShopRadar.Application.Abstractions.Parsers;

public interface IParser
{
    public Task<List<Offer>> ParseAsync();
}