using ShopRadar.Domain.Abstractions;
using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Domain.Products;

public sealed class Product : Entity<ProductId>
{
    private readonly List<Offer> _offers = [];

    private Product()
    {
    }

    private Product(ProductId id, CategoryId categoryId, Name name) : base(id)
    {
        CategoryId = categoryId;
        Name = name;
    }

    public CategoryId CategoryId { get; private set; }
    public Name Name { get; private set; }
    public IReadOnlyList<Offer> Offers => _offers;

    public static Result<Product> Create(CategoryId categoryId, Name name) =>
        new Product(ProductId.New(), categoryId, name);

    public Result AddOffer(Offer offer)
    {
        if (_offers.Any(o => o.Url == offer.Url))
        {
            return Result.Failure<Product>(ProductErrors.AlreadyExists);
        }

        _offers.Add(offer);
        return Result.Success();
    }
}