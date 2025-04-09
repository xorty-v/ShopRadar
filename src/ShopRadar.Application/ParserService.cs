using ShopRadar.Application.Abstractions;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Сategories;
using ShopRadar.Parsers;
using ShopRadar.Parsers.Abstractions;

namespace ShopRadar.Application;

internal sealed class ParserService : IParserService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IParserFactory _parserFactory;
    private readonly IProductRepository _productRepository;

    public ParserService(
        IParserFactory parserFactory,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _parserFactory = parserFactory;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task RunAllParser()
    {
        var shops = Enum.GetValues(typeof(StoreType)).Cast<StoreType>().ToList();

        var tasks = shops.Select(async shop =>
        {
            var parser = _parserFactory.CreateParser(shop);

            var categories = await parser.ParseCategoriesAsync();
            // await _categoryService.SaveCategoriesAsync(categories);

            var products = await parser.ParseProductsAsync(categories);
            // await _productService.SaveProductsAsync(products);
        });

        await Task.WhenAll(tasks);
    }
}