namespace ShopRadar.Domain.Products;

public sealed record ProductId
{
    private ProductId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ProductId New() => new(Guid.NewGuid());

    public static ProductId Create(Guid value) => new(value);
}