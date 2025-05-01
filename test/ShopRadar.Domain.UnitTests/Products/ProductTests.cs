using ShopRadar.Domain.Offers;
using ShopRadar.Domain.Products;
using ShopRadar.Domain.Stores;
using ShopRadar.Domain.UnitTests.Offers;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Domain.UnitTests.Products;

public class ProductTests
{
    [Fact]
    public void AddOffer_ShouldAdd_WhenNotDuplicate()
    {
        // Arrange
        var product = Product.Create(CategoryId.New(), ProductData.Name).Value;
        var offer = Offer.Create(null, StoreId.New(), OfferData.Name, OfferData.Url).Value;

        // Act
        product.AddOffer(offer);

        // Assert
        Assert.Single(product.Offers);
    }
}