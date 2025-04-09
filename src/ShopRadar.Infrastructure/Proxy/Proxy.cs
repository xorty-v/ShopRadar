namespace ShopRadar.Infrastructure.Proxy;

public record Proxy(
    string Username,
    string Password,
    string Proxy_Address,
    Dictionary<string, int> Ports
);