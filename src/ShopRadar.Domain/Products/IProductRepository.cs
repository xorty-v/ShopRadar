namespace ShopRadar.Domain.Products;

public interface IProductRepository
{
    public Task AddProductsAsync(List<Product> products, CancellationToken cancellationToken);
}