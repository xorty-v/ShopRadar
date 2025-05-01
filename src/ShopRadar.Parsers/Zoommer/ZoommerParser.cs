using System.Text.Json;
using System.Text.RegularExpressions;
using ShopRadar.Application.Abstractions.Loaders;
using ShopRadar.Application.Abstractions.Parsers;
using ShopRadar.Domain;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.PriceHistories;
using ShopRadar.Domain.Shared;
using ShopRadar.Parsers.Abstractions;
using ShopRadar.Parsers.Zoommer.JsonModels;

namespace ShopRadar.Parsers.Zoommer;

public class ZoommerParser : BaseParser, IParser
{
    private readonly IStaticPageLoader _staticPageLoader;

    public ZoommerParser(IStaticPageLoader staticPageLoader)
    {
        _staticPageLoader = staticPageLoader;
    }

    protected override string CategoryUrl { get; } = "https://api.zoommer.ge/v1/Categories/all-categories";
    protected override string ProductUrl { get; } = "https://api.zoommer.ge/v1/Products/v3?CategoryId=";

    public async Task<List<Offer>> ParseAsync()
    {
        var categories = await ParseCategoriesAsync();
        var offers = await ParseOffersAsync(categories);

        return offers;
    }

    public override async Task<List<CategoryRaw>> ParseCategoriesAsync()
    {
        var page = await _staticPageLoader.LoadPageAsync(CategoryUrl, HttpMethod.Get, null);
        var response = JsonSerializer.Deserialize<List<JsonCategory>>(page);

        var categories = response?.Select(category => new CategoryRaw
        {
            Name = category.Name,
            Url = category.Url
        }).ToList();
        List<CategoryRaw> test = new List<CategoryRaw>();
        test.Add(categories.FirstOrDefault());
        return test;
    }

    public override async Task<List<Offer>> ParseOffersAsync(List<CategoryRaw> categories)
    {
        var offers = new List<Offer>();
        var categoryUrls = categories.Select(c => Regex.Match(c.Url, @"\d+$").Value).ToList();

        var initialRequests = categoryUrls
            .Select(partUrl => ($"{ProductUrl}{partUrl}&Page=1&Limit=28", (HttpContent?)null))
            .ToList();

        var initialPages = await _staticPageLoader.LoadPagesAsync(initialRequests, HttpMethod.Get);
        var additionalRequests = new List<(string url, HttpContent? content)>();

        for (int i = 0; i < initialPages.Count; i++)
        {
            var pageContent = initialPages[i];
            if (string.IsNullOrWhiteSpace(pageContent)) continue;

            var response = JsonSerializer.Deserialize<JsonProductList>(pageContent);
            if (response?.Products == null) continue;

            offers.AddRange(MapOffers(response.Products));

            int totalPages = (int)Math.Ceiling(response.TotalCount / 28.0);
            if (totalPages <= 1) continue;

            for (int page = 2; page <= totalPages; page++)
            {
                additionalRequests.Add((
                    $"{ProductUrl}{categoryUrls[i]}&Page={page}&Limit=28",
                    null
                ));
            }
        }

        if (additionalRequests.Count > 0)
        {
            var additionalPages = await _staticPageLoader.LoadPagesAsync(additionalRequests, HttpMethod.Get);
            foreach (var page in additionalPages)
            {
                if (string.IsNullOrWhiteSpace(page)) continue;

                var response = JsonSerializer.Deserialize<JsonProductList>(page);
                if (response?.Products != null)
                {
                    offers.AddRange(MapOffers(response.Products));
                }
            }
        }

        return offers;
    }

    private IEnumerable<Offer> MapOffers(List<JsonProduct> jsonProducts)
    {
        foreach (var jsonProduct in jsonProducts)
        {
            var hasDiscount = jsonProduct.PreviousPrice.HasValue &&
                              jsonProduct.PreviousPrice.Value > jsonProduct.ActualPrice;

            var offerResult = Offer.Create(
                null,
                Constants.PredefinedIds.Stores.Zoommer,
                Name.Create(jsonProduct.Name).Value,
                Url.Create("https://zoommer.ge/en/" + jsonProduct.Route).Value,
                Money.Create(hasDiscount ? jsonProduct.PreviousPrice.Value : jsonProduct.ActualPrice),
                hasDiscount ? Money.Create(jsonProduct.ActualPrice) : Money.Zero(),
                DateTime.UtcNow
            );

            yield return offerResult.Value;
        }
    }
}