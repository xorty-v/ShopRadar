using Microsoft.EntityFrameworkCore;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Infrastructure.Repositories;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddCategories(List<Category> categories, CancellationToken cancellationToken)
    {
        _dbContext.Categories.AddRange(categories);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Category>> GetCategoriesByShopIdAsync(Guid storeId, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.Where(c => c.StoreId == storeId).ToListAsync(cancellationToken);
    }
}