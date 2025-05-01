using ShopRadar.Domain.Offers;
using ShopRadar.Domain.PriceHistories;
using ShopRadar.Domain.Stores;

namespace ShopRadar.Domain.UnitTests.Offers;

public class OfferTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenValid()
    {
        // Act
        var result = Offer.Create(null, StoreId.New(), OfferData.Name, OfferData.Url);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void UpdatePrice_ShouldAddNewPriceHistory_WhenPriceChanges()
    {
        // Arrange
        var offer = Offer.Create(null, StoreId.New(), OfferData.Name, OfferData.Url).Value;

        // Act
        offer.UpdatePrice(Money.Create(100), Money.Create(90), DateTime.UtcNow);
        offer.UpdatePrice(Money.Create(110), Money.Create(95), DateTime.UtcNow.AddDays(1));

        // Assert
        Assert.Equal(2, offer.PriceHistories.Count);
    }

    [Fact]
    public void UpdatePrice_ShouldNotAdd_WhenPriceUnchanged()
    {
        // Arrange
        var offer = Offer.Create(null, StoreId.New(), OfferData.Name, OfferData.Url).Value;

        // Act
        offer.UpdatePrice(Money.Create(100), Money.Create(90), DateTime.UtcNow);
        offer.UpdatePrice(Money.Create(100), Money.Create(90), DateTime.UtcNow.AddDays(1));

        // Assert
        Assert.Single(offer.PriceHistories);
    }
}