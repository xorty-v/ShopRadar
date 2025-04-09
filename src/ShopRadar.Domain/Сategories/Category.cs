using ShopRadar.Domain.Products;
using ShopRadar.Domain.Stores;

namespace ShopRadar.Domain.Сategories;

public sealed class Category
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }

    public Store Store { get; set; }
    public ICollection<Product> Products { get; set; }
}