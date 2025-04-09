using System.Net;

namespace ShopRadar.Infrastructure.Proxy;

public interface IProxyProvider
{
    public Task<WebProxy> GetProxyAsync();
}