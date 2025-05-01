using ShopRadar.Domain.Stores;
using ShopRadar.Domain.Сategories;

namespace ShopRadar.Domain;

public static class Constants
{
    public static class PredefinedIds
    {
        public static class Stores
        {
            public static StoreId Alta = StoreId.Create(Guid.Parse("99b698b7-ae46-4a2c-af66-d7155497caff"));
            public static StoreId Zoommer = StoreId.Create(Guid.Parse("2509a6ea-b884-4f49-8b44-af7da9b7812c"));
            public static StoreId EliteElectronic = StoreId.Create(Guid.Parse("8d779e8f-f6c5-46da-9eeb-5f749c7988ba"));
        }

        public static class Categories
        {
            public static CategoryId Laptops = CategoryId.Create(Guid.Parse("bae1a401-84a9-4466-a8c3-aa466d3d3af0"));

            public static CategoryId Smartphones =
                CategoryId.Create(Guid.Parse("90f90635-ddfc-4e8b-83d6-b723e5e6a46f"));

            public static CategoryId Headphones = CategoryId.Create(Guid.Parse("c6b1741f-2c71-4bfc-9d9e-b87b9cafbf61"));
            public static CategoryId Keyboards = CategoryId.Create(Guid.Parse("aab69d60-609c-41be-8bd0-e8b90d4c43fd"));
            public static CategoryId Monitors = CategoryId.Create(Guid.Parse("fc2f9d79-82b3-4305-a0b1-7341f99ba26c"));
        }
    }
}