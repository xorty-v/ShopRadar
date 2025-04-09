using ShopRadar.Domain.Products;
using ShopRadar.Domain.Сategories;
using ShopRadar.Parsers.Abstractions;

namespace ShopRadar.Parsers.Alta;

public class AltaParser : IParser
{
    public Task<List<Category>> ParseCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Product>> ParseProductsAsync(List<Category> categories)
    {
        throw new NotImplementedException();
    }
}