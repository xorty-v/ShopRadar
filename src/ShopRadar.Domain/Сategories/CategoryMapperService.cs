namespace ShopRadar.Domain.Сategories;

public class CategoryMapperService
{
    private readonly Lazy<Dictionary<Guid, List<string>>> _categoryMappings;

    public CategoryMapperService()
    {
        _categoryMappings = new Lazy<Dictionary<Guid, List<string>>>(GetDefaultCategoryMap());
    }

    public Guid MapCategory(string storeCategory)
    {
        var input = storeCategory.Trim();

        foreach (var pair in _categoryMappings.Value)
        {
            if (pair.Value.Any(alias => string.Equals(alias, input, StringComparison.OrdinalIgnoreCase)))
            {
                return pair.Key;
            }
        }

        return Guid.Empty;
    }

    private static Dictionary<Guid, List<string>> GetDefaultCategoryMap()
    {
        return new Dictionary<Guid, List<string>>
        {
            {
                Constants.PredefinedIds.Categories.Laptops.Value, new List<string>
                {
                    "Notebooks",
                    "Laptop brands",
                    "note-pc"
                }
            },
            {
                Constants.PredefinedIds.Categories.Smartphones.Value, new List<string>
                {
                    "Smartphones",
                    "Mobile Phones",
                    "Mobile Phone"
                }
            },
            {
                Constants.PredefinedIds.Categories.Monitors.Value, new List<string>
                {
                    "Monitors",
                    "Monitors",
                    "Monitor"
                }
            },
            {
                Constants.PredefinedIds.Categories.Headphones.Value, new List<string>
                {
                    "Headphones",
                    "Headphones & Headsets",
                    "Headphones"
                }
            },
            {
                Constants.PredefinedIds.Categories.Keyboards.Value, new List<string>
                {
                    "Keyboards",
                    "Keyboards",
                    "Keyboard"
                }
            }
        };
    }
}