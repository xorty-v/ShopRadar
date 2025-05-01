namespace ShopRadar.Domain.Stores;

public sealed record StoreId
{
    private StoreId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static StoreId New() => new(Guid.NewGuid());

    public static StoreId Create(Guid value) => new(value);
}