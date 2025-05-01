namespace ShopRadar.Parsers.Abstractions;

public record CategoryRaw
{
    public string Name { get; init; }
    public string Url { get; init; }
}