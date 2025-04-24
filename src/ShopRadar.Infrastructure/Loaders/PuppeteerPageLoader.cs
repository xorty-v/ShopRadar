using System.Net;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using ShopRadar.Application.Abstractions.Loaders;
using ShopRadar.Infrastructure.Proxy;

namespace ShopRadar.Infrastructure.Loaders;

internal sealed class PuppeteerPageLoader : BrowserPageLoader, IBrowserPageLoader
{
    private readonly IProxyProvider _proxyProvider;
    private readonly SemaphoreSlim _semaphore = new(5);

    public PuppeteerPageLoader(IProxyProvider proxyProvider)
    {
        _proxyProvider = proxyProvider;
    }

    public async Task<string> LoadPageAsync(string url, List<PageAction>? pageActions = null, bool headless = true)
    {
        var puppeteerExtra = new PuppeteerExtra().Use(new AnonymizeUaPlugin()).Use(new StealthPlugin());

        var proxy = await _proxyProvider.GetProxyAsync();
        var launchOptions = ConfigureLaunchAsync(headless, proxy);

        await using var browser = await puppeteerExtra.LaunchAsync(launchOptions);

        await using var page = await browser.NewPageAsync();
        await ConfigurePageAsync(page, proxy);

        await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);

        if (pageActions != null)
            foreach (var action in pageActions)
                await PageActions[action.Type](page, action.Parameters);

        var html = await page.GetContentAsync();

        return html;
    }

    public async Task<IReadOnlyList<string?>> LoadPagesAsync(IEnumerable<string> urls, List<PageAction>? pageActions)
    {
        var puppeteerExtra = new PuppeteerExtra().Use(new AnonymizeUaPlugin()).Use(new StealthPlugin());

        await _semaphore.WaitAsync();

        try
        {
            var proxy = await _proxyProvider.GetProxyAsync();
            var launchOptions = ConfigureLaunchAsync(true, proxy);

            await using var browser = await puppeteerExtra.LaunchAsync(launchOptions);

            var tasks = new List<Task<string?>>();
            foreach (var url in urls)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await using var page = await browser.NewPageAsync();
                        await ConfigurePageAsync(page, proxy);

                        await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);

                        if (pageActions != null)
                        {
                            foreach (var action in pageActions)
                            {
                                if (PageActions.TryGetValue(action.Type, out var handler))
                                    await handler(page, action.Parameters);
                            }
                        }

                        return await page.GetContentAsync();
                    }
                    catch
                    {
                        return null;
                    }
                }));
            }

            return await Task.WhenAll(tasks);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    #region Configuring Browser Settings

    private LaunchOptions ConfigureLaunchAsync(
        bool headless,
        WebProxy proxy)
    {
        var proxyAddress = $"--proxy-server={proxy.Address!.Host}:{proxy.Address.Port}";

        return new LaunchOptions
        {
            Headless = headless,
            Args = new[]
            {
                proxyAddress,
                "--disable-blink-features=AutomationControlled",
                "--disable-infobars",
                "--disable-extensions",
                "--no-first-run",
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage",
                "--no-zygote"
            },
            IgnoredDefaultArgs = new[] { "enable-automation" }
        };
    }

    private async Task ConfigurePageAsync(IPage page, WebProxy proxy)
    {
        await page.SetViewportAsync(new ViewPortOptions { Width = 1280, Height = 800 });

        await page.SetRequestInterceptionAsync(true);
        page.Request += (_, e) =>
        {
            if (e.Request.ResourceType == ResourceType.Image ||
                e.Request.ResourceType == ResourceType.StyleSheet ||
                e.Request.ResourceType == ResourceType.Media ||
                e.Request.ResourceType == ResourceType.Font)
            {
                e.Request.AbortAsync();
            }
            else
            {
                e.Request.ContinueAsync();
            }
        };

        var creds = proxy.Credentials?.GetCredential(new Uri(proxy.Address!.ToString()), string.Empty);

        await page.AuthenticateAsync(new Credentials
        {
            Username = creds?.UserName,
            Password = creds?.Password
        });
    }

    #endregion
}