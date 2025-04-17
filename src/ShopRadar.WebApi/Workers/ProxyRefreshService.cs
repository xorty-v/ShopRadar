using ShopRadar.Infrastructure.Proxy;

namespace ShopRadar.WebApi.Workers;

internal sealed class ProxyRefreshService : BackgroundService
{
    private readonly IProxyProvider _proxyProvider;

    public ProxyRefreshService(IProxyProvider proxyProvider)
    {
        _proxyProvider = proxyProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _proxyProvider.RefreshProxiesAsync();

            await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
        }
    }
}