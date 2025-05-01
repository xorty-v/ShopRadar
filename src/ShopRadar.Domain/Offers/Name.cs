using ShopRadar.Domain.Abstractions;

namespace ShopRadar.Domain.Offers;

public sealed record Name
{
    public static readonly Error Empty = new("Name.Empty", "Name can not be empty");

    private Name(string value) => Value = value;

    public string Value { get; }

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Name>(Empty);
        }

        return new Name(value);
    }
}