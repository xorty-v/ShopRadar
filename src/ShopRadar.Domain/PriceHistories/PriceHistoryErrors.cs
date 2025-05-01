using ShopRadar.Domain.Abstractions;

namespace ShopRadar.Domain.PriceHistories;

public static class PriceHistoryErrors
{
    public static readonly Error DiscountExceedsBasePrice = new(
        "Review.DiscountExceedsBase",
        "The discount price can not be greater than the base price");
}