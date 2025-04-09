namespace ShopRadar.Domain.Сategories;

public interface ICategoryRepository
{
    public Task AddCategories(List<Category> categories, CancellationToken cancellationToken);
    public Task<List<Category>> GetCategoriesByShopIdAsync(Guid storeId, CancellationToken cancellationToken);
}