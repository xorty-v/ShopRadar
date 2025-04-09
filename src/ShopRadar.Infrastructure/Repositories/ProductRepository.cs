using ShopRadar.Domain.Products;

namespace ShopRadar.Infrastructure.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddProductsAsync(List<Product> products, CancellationToken cancellationToken)
    {
        _dbContext.Products.AddRange(products);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}