namespace ShopRadar.Domain.Offers;

public sealed record OfferId
{
    private OfferId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static OfferId New() => new(Guid.NewGuid());

    public static OfferId Create(Guid value) => new(value);
}