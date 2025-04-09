using System.Net;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using ShopRadar.Infrastructure.Proxy;

namespace ShopRadar.Infrastructure.PageFetchers;

public class BrowserPageFetcher : IPageFetcher
{
    private const int MaxConcurrentBrowsers = 5;
    private const int MaxLinksPerBrowser = 20;
    private readonly IProxyProvider _proxyProvider;

    public BrowserPageFetcher(IProxyProvider proxyProvider)
    {
        _proxyProvider = proxyProvider;
    }

    public async Task<List<string>> FetchPagesAsync(List<FetchRequest> requests, CancellationToken cancellationToken)
    {
        await new BrowserFetcher().DownloadAsync();
        var puppeteerExtra = new PuppeteerExtra().Use(new AnonymizeUaPlugin()).Use(new StealthPlugin());

        var urlBatches = requests.Chunk(MaxLinksPerBrowser).ToList();
        var semaphore = new SemaphoreSlim(MaxConcurrentBrowsers);
        var tasks = new List<Task<List<string>>>();

        foreach (var batch in urlBatches)
        {
            await semaphore.WaitAsync(cancellationToken);
            tasks.Add(ProcessBatchAsync(batch, puppeteerExtra, semaphore, cancellationToken));
        }

        var results = await Task.WhenAll(tasks);
        return results.SelectMany(x => x).ToList();
    }

    private async Task<List<string>> ProcessBatchAsync(
        FetchRequest[] batch,
        PuppeteerExtra puppeteerExtra,
        SemaphoreSlim semaphore,
        CancellationToken cancellationToken)
    {
        try
        {
            var options = await GetLaunchOptions();
            await using var browser = await puppeteerExtra.LaunchAsync(options);

            var htmlResults = new List<string>();
            foreach (var request in batch)
            {
                try
                {
                    await using var page = await browser.NewPageAsync();
                    await ConfigurePageSettingsAsync(page);
                    await page.GoToAsync(request.Url, WaitUntilNavigation.DOMContentLoaded);

                    htmlResults.Add(await page.GetContentAsync());
                }
                catch (Exception ex)
                {
                    //TODO: Добавить логгер - $"Ошибка при загрузке {url}: {ex.Message}"
                    htmlResults.Add(null);
                }

                await Task.Delay(Random.Shared.Next(1500, 5000), cancellationToken);
            }

            return htmlResults;
        }
        finally
        {
            semaphore.Release();
        }
    }

    #region Configuring Browser Settings

    private async Task ConfigurePageSettingsAsync(IPage page)
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
    }

    private async Task<LaunchOptions> GetLaunchOptions()
    {
        WebProxy proxy = await _proxyProvider.GetProxyAsync();

        return new LaunchOptions
        {
            Headless = false,
            Args = GetChromeArguments($"{proxy.Address!.Host}:{proxy.Address.Port}"),
            IgnoredDefaultArgs = new[] { "enable-automation" }
        };
    }

    private string[] GetChromeArguments(string? proxy)
    {
        return new[]
        {
            $"--proxy-server={proxy}",
            "--disable-gpu",
            "--no-zygote",
            "--disable-blink-features=AutomationControlled",
            "--deterministic-fetch",
            "--disable-infobars",
            "--start-maximized",
            "--no-first-run",
            "--disable-extensions",
            "--disable-features=IsolateOrigins,SiteIsolationTrials",
            "--no-sandbox",
            "--disable-setuid-sandbox",
            "--disable-dev-shm-usage"
        };
    }

    #endregion
}