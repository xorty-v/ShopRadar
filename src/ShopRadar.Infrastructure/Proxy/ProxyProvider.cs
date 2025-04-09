using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShopRadar.Infrastructure.Proxy;

internal sealed class ProxyProvider : IProxyProvider
{
    private const string ProxyApiKey = "";
    private const string GetProxiesUrl = "https://proxy.webshare.io/api/proxy/list/";
    private static readonly HttpClient HttpClient = new HttpClient();

    private Task _initialization;
    private Random _rnd = new Random();

    private List<WebProxy> _webProxies = new List<WebProxy>();

    public ProxyProvider()
    {
        _initialization = InitAsync();
    }

    public async Task<WebProxy> GetProxyAsync()
    {
        await _initialization;

        int index = _rnd.Next(0, _webProxies.Count);

        return _webProxies[index];
    }

    private async Task InitAsync()
    {
        var proxies = await GetWebProxies();
        _webProxies.AddRange(proxies);
    }

    private async Task<List<Proxy>> GetProxies()
    {
        var proxies = new List<Proxy>();

        HttpClient.DefaultRequestHeaders.Add("Authorization", ProxyApiKey);

        var response = await HttpClient.GetAsync(GetProxiesUrl);

        JObject json = JObject.Parse(await response.Content.ReadAsStringAsync());
        List<Proxy>? deserializepProxies = JsonConvert.DeserializeObject<List<Proxy>>(json["results"]!.ToString());

        if (deserializepProxies != null)
        {
            proxies.AddRange(deserializepProxies);
        }

        return proxies;
    }

    private async Task<List<WebProxy>> GetWebProxies()
    {
        var rawProxies = await GetProxies();

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
}