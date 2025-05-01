using ShopRadar.Domain.Products;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Domain.UnitTests.Сategories;

public class CategoryTests
{
    [Fact]
    public void AddProduct_ShouldAdd_WhenValid()
    {
        // Arrange
        var category = Category.Create(CategoryId.New(), "Electronics");
        var product = Product.Create(category.Id, Name.Create("TV").Value).Value;

        // Act
        category.AddProduct(product);

        // Assert
        Assert.Single(category.Products);
    }
}