using ShopRadar.Domain.Abstractions;

namespace ShopRadar.Domain.Shared;

public sealed record Url
{
    public static readonly Error Empty = new("Url.Empty", "Url cannot be empty");

    private Url(string value) => Value = value;

    public string Value { get; init; }

    public static Result<Url> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Url>(Empty);
        }

        return new Url(value);
    }
}