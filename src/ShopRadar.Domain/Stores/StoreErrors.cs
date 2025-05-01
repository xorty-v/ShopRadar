using ShopRadar.Domain.Abstractions;

namespace ShopRadar.Domain.Stores;

public static class StoreErrors
{
    public static readonly Error AlreadyExists = new(
        "Store.AlreadyExists",
        "The offer has already existed");
}