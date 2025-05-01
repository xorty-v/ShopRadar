using System.Text.Json;
using AngleSharp;
using ShopRadar.Application.Abstractions.Loaders;
using ShopRadar.Application.Abstractions.Parsers;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.PriceHistories;
using ShopRadar.Parsers.Abstractions;
using ShopRadar.Parsers.EliteElectronic.JsonModels;
using ShopRadar.Parsers.EliteElectronic.Requests;
using Constants = ShopRadar.Domain.Constants;
using Url = ShopRadar.Domain.Shared.Url;

namespace ShopRadar.Parsers.EliteElectronic;

public class EliteElectronicParser : BaseParser, IParser
{
    private readonly IBrowsingContext _browsingContext;
    private readonly IStaticPageLoader _staticPageLoader;

    public EliteElectronicParser(IStaticPageLoader staticPageLoader, IBrowsingContext browsingContext)
    {
        _staticPageLoader = staticPageLoader;
        _browsingContext = browsingContext;
    }

    protected override string CategoryUrl { get; } = "https://ee.ge/";
    protected override string ProductUrl { get; } = "https://api.ee.ge/product/filter_products";

    public async Task<List<Offer>> ParseAsync()
    {
        var categories = await ParseCategoriesAsync();
        var offers = await ParseOffersAsync(categories);

        return offers;
    }

    public override async Task<List<CategoryRaw>> ParseCategoriesAsync()
    {
        var pageContent = await _staticPageLoader.LoadPageAsync(CategoryUrl, HttpMethod.Get, null);
        var document = await _browsingContext.OpenAsync(req => req.Content(pageContent));

        var categories = document
            .QuerySelectorAll("div.dropdown-content a")
            .Select(a => new CategoryRaw
            {
                Name = a.TextContent.Trim(),
                Url = a.GetAttribute("href")
            }).ToList();

        if (categories == null)
        {
            throw new InvalidOperationException("No valid category found");
        }

        List<CategoryRaw> test = new List<CategoryRaw>();
        test.Add(categories.FirstOrDefault());
        return test;
    }

    public override async Task<List<Offer>> ParseOffersAsync(List<CategoryRaw> categories)
    {
        var offers = new List<Offer>();
        var categoryUrls = categories.Select(c => c.Url.Split('/').Last()).ToList();

        var initialRequests = categoryUrls
            .Select(url => (ProductUrl, PostRequestBody.Create(url)))
            .ToList();

        var initialPages = await _staticPageLoader.LoadPagesAsync(initialRequests, HttpMethod.Post);
        var additionalRequests = new List<(string url, HttpContent content)>();

        for (int i = 0; i < initialPages.Count; i++)
        {
            var pageContent = initialPages[i];
            if (string.IsNullOrWhiteSpace(pageContent)) continue;

            var response = JsonSerializer.Deserialize<JsonProductList>(pageContent);
            if (response?.Products == null) continue;

            offers.AddRange(MapOffers(response.Products));

            int totalPages = (int)Math.Ceiling(response.TotalCount / 10.0);
            if (totalPages <= 1) continue;

            for (int page = 2; page <= totalPages; page++)
            {
                additionalRequests.Add((ProductUrl, PostRequestBody.Create(categoryUrls[i], page)));
            }
        }

        if (additionalRequests.Count > 0)
        {
            var additionalPages = await _staticPageLoader.LoadPagesAsync(additionalRequests, HttpMethod.Post);

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
            var offerResult = Offer.Create(
                null,
                Constants.PredefinedIds.Stores.EliteElectronic,
                Name.Create(jsonProduct.Name).Value,
                Url.Create(
                        $"https://ee.ge/{jsonProduct.ParentCategory}/{jsonProduct.Category}/{jsonProduct.ProductSlug}")
                    .Value,
                Money.Create(jsonProduct.ActualPrice),
                Money.Create(jsonProduct.SalePrice),
                DateTime.UtcNow
            );

            yield return offerResult.Value;
        }
    }
}