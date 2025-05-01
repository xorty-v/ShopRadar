using ShopRadar.Domain.Offers;
using ShopRadar.Domain.PriceHistories;

namespace ShopRadar.Domain.UnitTests.PriceHistories;

public class PriceHistoryTests
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenDiscountGreaterThanPrice()
    {
        // Arrange
        var price = Money.Create(100);
        var discountPrice = Money.Create(110);

        // Act
        var result = PriceHistory.Create(OfferId.New(), price, discountPrice, DateTime.UtcNow);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var price = Money.Create(100);
        var discountPrice = Money.Create(90);

        // Act
        var result = PriceHistory.Create(OfferId.New(), price, discountPrice, DateTime.UtcNow);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}