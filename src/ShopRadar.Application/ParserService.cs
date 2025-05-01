using ShopRadar.Application.Abstractions;
using ShopRadar.Application.Abstractions.Parsers;
using ShopRadar.Domain.Offers;

namespace ShopRadar.Application;

internal sealed class ParserService : IParserService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IParserFactory _parserFactory;

    public ParserService(IParserFactory parserFactory, IOfferRepository offerRepository)
    {
        _parserFactory = parserFactory;
        _offerRepository = offerRepository;
    }

    public async Task RunAllParsers()
    {
        var shops = Enum.GetValues<StoreType>().ToList();

        var tasks = shops.Select(async shop =>
        {
            var parser = _parserFactory.CreateParser(shop);

            var offers = await parser.ParseAsync();

            await _offerRepository.AddOffersAsync(offers.ToList());
        });

        await Task.WhenAll(tasks);
    }
}