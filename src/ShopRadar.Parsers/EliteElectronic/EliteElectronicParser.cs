using System.Text.Json;
using AngleSharp;
using ShopRadar.Domain.Сategories;
using ShopRadar.Infrastructure.PageFetchers;
using ShopRadar.Parsers.Abstractions;
using ShopRadar.Parsers.EliteElectronic.JsonModels;
using Product = ShopRadar.Domain.Products.Product;

namespace ShopRadar.Parsers.EliteElectronic;

public class EliteElectronicParser : IParser
{
    private readonly IBrowsingContext _browsingContext;
    private readonly IPageFetcher _pageFetcher;

    public EliteElectronicParser(IPageFetcher pageFetcher, IBrowsingContext browsingContext)
    {
        _pageFetcher = pageFetcher;
        _browsingContext = browsingContext;
    }

    public async Task<List<Category>> ParseCategoriesAsync()
    {
        var request = new FetchRequest(EliteElectronicSettings.BaseUrl, HttpMethod.Get);
        var page = await _pageFetcher.FetchPagesAsync(new List<FetchRequest> { request });
        var document = await _browsingContext.OpenAsync(req => req.Content(page[0]));

        var сategories = document
            .QuerySelectorAll("div.dropdown-content a")
            .Select(a => new Category
            {
                Id = Guid.NewGuid(),
                Name = a.TextContent.Trim(),
                Url = a.GetAttribute("href")!
            })
            .Where(c => !c.Url.Contains("/furniture/", StringComparison.OrdinalIgnoreCase))
            .ToList();

        return сategories;
    }

    public async Task<List<Product>> ParseProductsAsync(List<Category> categories)
    {
        var products = new List<Product>();
        var fetchRequests = new List<FetchRequest>();

        foreach (var category in categories)
        {
            string categoryId = category.Url.Split('/').Last();

            fetchRequests.Add(new FetchRequest(
                EliteElectronicSettings.ApiUrl,
                HttpMethod.Post,
                CreateRequestBody(categoryId)
            ));
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

            int totalPages = (int)Math.Ceiling(response.TotalCount / 10.0);
            if (totalPages <= 1) continue;

            string categoryId = categories[i].Url.Split('/').Last();
            for (int page = 2; page <= totalPages; page++)
            {
                additionalRequests.Add(new FetchRequest(
                    EliteElectronicSettings.ApiUrl,
                    HttpMethod.Post,
                    CreateRequestBody(categoryId, page)
                ));
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

    private static object CreateRequestBody(string category, int page = 1, int itemsPerPage = 10)
    {
        var requestBody = new
        {
            min_price = 0,
            max_price = 0,
            category = category,
            brand = new string[] { },
            color = new string[] { },
            room = new string[] { },
            sort_by = "",
            item_per_page = itemsPerPage,
            page_no = page,
            specification = new string[] { },
            sale_products = 0,
            search_text = "",
            slug = "",
            pageno = (int?)null
        };

        return requestBody;
    }

    private List<Product> MapProducts(List<JsonProduct> jsonProducts) =>
        jsonProducts.Select(jsonProduct =>
            new Product
            {
                Id = Guid.NewGuid(),
                CategoryId = Guid.Parse("d64cdc03-3b7d-4bdc-af20-c7e8a4978f9b"),
                Name = jsonProduct.Name,
                Price = jsonProduct.ActualPrice,
                DiscountPrice = jsonProduct.SalePrice,
                Url = "test.com"
            }).ToList();
}