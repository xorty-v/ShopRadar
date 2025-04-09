using System.Text.Json;
using System.Text.RegularExpressions;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Сategories;
using ShopRadar.Infrastructure.PageFetchers;
using ShopRadar.Parsers.Abstractions;
using ShopRadar.Parsers.Zoommer.JsonModels;

namespace ShopRadar.Parsers.Zoommer;

public class ZoommerParser : IParser
{
    private readonly IPageFetcher _pageFetcher;

    public ZoommerParser(IPageFetcher pageFetcher)
    {
        _pageFetcher = pageFetcher;
    }

    public async Task<List<Category>> ParseCategoriesAsync()
    {
        var request = new FetchRequest(ZoommerSettings.CategoryUrl, HttpMethod.Get);
        var page = await _pageFetcher.FetchPagesAsync(new List<FetchRequest> { request });
        var response = JsonSerializer.Deserialize<List<JsonCategory>>(page[0]);

        var categories = new List<Category>();
        foreach (var category in response)
        {
            categories.Add(new Category { Name = category.Name, Url = category.Url });
        }

        return categories;
    }

    public async Task<List<Product>> ParseProductsAsync(List<Category> categories)
    {
        var products = new List<Product>();

        var fetchRequests = new List<FetchRequest>();
        foreach (var category in categories)
        {
            string categoryId = Regex.Match(category.Url, @"\d+$").Value;

            fetchRequests.Add(new FetchRequest(
                $"{ZoommerSettings.ProductUrl}{categoryId}&Page=1&Limit=28",
                HttpMethod.Get)
            );
        }

        var initialPages = await _pageFetcher.FetchPagesAsync(fetchRequests);
        var additionalRequests = new List<FetchRequest>();

        for (int i = 0; i < categories.Count; i++)
        {
            var response = JsonSerializer.Deserialize<JsonProductList>(initialPages[i]);

            if (response == null)
            {
                continue;
            }

            products.AddRange(MapProducts(response.Products!));

            int totalPages = (int)Math.Ceiling(response.TotalCount / 28.0);
            if (totalPages <= 1) continue;

            string categoryId = Regex.Match(categories[i].Url, @"\d+$").Value;
            for (int page = 2; page <= totalPages; page++)
            {
                additionalRequests.Add(new FetchRequest(
                    $"{ZoommerSettings.ProductUrl}{categoryId}&Page={page}&Limit=28",
                    HttpMethod.Get)
                );
            }
        }

        var remainingPages = await _pageFetcher.FetchPagesAsync(additionalRequests);
        foreach (var page in remainingPages)
        {
            var response = JsonSerializer.Deserialize<JsonProductList>(page);

            if (response != null)
            {
                products.AddRange(MapProducts(response.Products!));
            }
        }

        return products;
    }

    private List<Product> MapProducts(List<JsonProduct> jsonProducts) =>
        jsonProducts.Select(jsonProduct =>
        {
            var hasDiscount = jsonProduct.PreviousPrice.HasValue &&
                              jsonProduct.PreviousPrice.Value > jsonProduct.ActualPrice;

            return new Product
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.Parse("d64cdc03-3b7d-4bdc-af20-c7e8a4978f9b"),
                Name = jsonProduct.Name,
                Price = hasDiscount ? jsonProduct.PreviousPrice.Value : jsonProduct.ActualPrice,
                DiscountPrice = hasDiscount ? jsonProduct.ActualPrice : null,
                Url = "test2.com"
            };
        }).ToList();
}