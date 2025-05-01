using ShopRadar.Domain.Abstractions;
using ShopRadar.Domain.Products;

namespace ShopRadar.Domain.Сategories;

public sealed class Category : Entity<CategoryId>
{
    private readonly List<Product> _products = [];

    private Category()
    {
    }

    private Category(CategoryId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }
    public IReadOnlyList<Product> Products => _products;

    public static Category Create(CategoryId id, string name) => new(id, name);

    public void AddProduct(Product product) => _products.Add(product);
}