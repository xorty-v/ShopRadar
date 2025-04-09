using ShopRadar.Domain.Сategories;

namespace ShopRadar.Domain.Stores;

public sealed class Store
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }

    public ICollection<Category> Categories { get; set; }
}