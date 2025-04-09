using ShopRadar.Domain.Products;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Parsers.Abstractions;

public interface IParser
{
    public Task<List<Category>> ParseCategoriesAsync();

    public Task<List<Product>> ParseProductsAsync(List<Category> categories);
    // TODO: Task<ProductDetails> ParseProductDetailsAsync();
}