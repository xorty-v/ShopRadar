using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Stores;
using ShopRadar.Domain.UnitTests.Offers;

namespace ShopRadar.Domain.UnitTests.Stores;

public class StoreTests
{
    [Fact]
    public void AddOffer_ShouldAdd_WhenValid()
    {
        // Arrange
        var store = Store.Create(StoreId.New(), "Test", StoreData.Url);
        var offer = Offer.Create(null, StoreId.New(), OfferData.Name, OfferData.Url).Value;

        // Act
        store.AddOffer(offer);

        // Assert
        Assert.Single(store.Offers);
    }
}