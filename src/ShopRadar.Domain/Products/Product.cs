using ShopRadar.Domain.Сategories;

namespace ShopRadar.Domain.Products;

public sealed class Product
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string Url { get; set; }

    public Category Category { get; set; }
}