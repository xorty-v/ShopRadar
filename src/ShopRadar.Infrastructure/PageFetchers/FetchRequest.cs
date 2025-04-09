using System.Text.Json;

namespace ShopRadar.Infrastructure.PageFetchers;

public sealed record FetchRequest(string Url, HttpMethod? Method, object? Body = null)
{
    public string? GetBodyAsString() => Body switch
    {
        null => null,
        string str => str,
        _ => JsonSerializer.Serialize(Body)
    };
}