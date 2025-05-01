namespace ShopRadar.Domain.PriceHistories;

public sealed record PriceHistoryId
{
    private PriceHistoryId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PriceHistoryId New() => new(Guid.NewGuid());

    public static PriceHistoryId Create(Guid value) => new(value);
}