using ShopRadar.Domain.Abstractions;

namespace ShopRadar.Domain.Products;

public static class ProductErrors
{
    public static readonly Error AlreadyExists = new(
        "Product.AlreadyExists",
        "The offer has already existed");
}