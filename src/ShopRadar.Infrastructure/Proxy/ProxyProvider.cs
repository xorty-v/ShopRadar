using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShopRadar.Infrastructure.Helpers;

namespace ShopRadar.Infrastructure.Proxy;

internal sealed class ProxyProvider : IProxyProvider
{
    private readonly HttpClient _httpClient;
    private readonly Lock _lock = new();

    private int _currentIndex = -1;
    private List<WebProxy> _webProxies = new();

    public ProxyProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.WebshareClient);
    }

    public Task<WebProxy> GetProxyAsync()
    {
        WebProxy proxy;

        using (_lock.EnterScope())
        {
            proxy = _webProxies[(_currentIndex + 1) % _webProxies.Count];
        }

        return Task.FromResult(proxy);
    }

    public async Task RefreshProxiesAsync()
    {
        var proxies = await GetWebProxiesAsync();

        using (_lock.EnterScope())
        {
            if (!proxies.Except(_webProxies).Any())
            {
                return;
            }

            _webProxies = proxies;
            _currentIndex = -1;
        }
    }

    private async Task<List<WebProxy>> GetWebProxiesAsync()
    {
        var rawProxies = await GetProxiesAsync();

        var proxies = rawProxies.Select(p => new WebProxy
        {
            Address = new Uri($"http://{p.Proxy_Address}:{p.Ports["http"]}"),
            BypassProxyOnLocal = false,
            UseDefaultCredentials = false,

            Credentials = new NetworkCredential(
                userName: p.Username,
                password: p.Password)
        });

        return proxies.ToList();
    }

    private async Task<List<Proxy>> GetProxiesAsync()
    {
        var proxies = new List<Proxy>();

        var response = await _httpClient.GetAsync("proxy/list/");

        JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());
        List<Proxy>? deserializepProxies = JsonConvert.DeserializeObject<List<Proxy>>(json["results"]!.ToString());

        if (deserializepProxies != null)
        {
            proxies.AddRange(deserializepProxies);
        }

        return proxies;
    }
}