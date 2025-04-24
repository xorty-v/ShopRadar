namespace ShopRadar.Infrastructure.Proxy;

public record ProxySettings
{
    public string BaseUrl { get; init; }
    public string ApiKey { get; init; }
    public int Timeout { get; init; }
}