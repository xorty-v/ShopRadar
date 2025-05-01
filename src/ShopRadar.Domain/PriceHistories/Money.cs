namespace ShopRadar.Domain.PriceHistories;

public sealed record Money
{
    private Money(decimal value) => Value = value;

    public decimal Value { get; init; }

    public static Money Create(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidOperationException("Amount can not be negative");
        }

        return new Money(value);
    }

    public static Money Zero() => new(0);

    public static bool operator >(Money left, Money right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(Money left, Money right)
    {
        return left.Value < right.Value;
    }
}