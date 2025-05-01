namespace ShopRadar.Domain.Сategories;

public sealed record CategoryId
{
    private CategoryId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CategoryId New() => new(Guid.NewGuid());

    public static CategoryId Create(Guid value) => new(value);
}