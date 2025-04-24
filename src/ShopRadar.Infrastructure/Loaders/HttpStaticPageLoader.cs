using ShopRadar.Application.Abstractions.Loaders;
using ShopRadar.Infrastructure.Helpers;

namespace ShopRadar.Infrastructure.Loaders;

internal sealed class HttpStaticPageLoader : IStaticPageLoader
{
    private readonly HttpClient _httpClient;
    private readonly SemaphoreSlim _semaphore = new(5);

    public HttpStaticPageLoader(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.PageLoader);
    }

    public async Task<string> LoadPageAsync(string url, HttpMethod method, HttpContent? content)
    {
        var request = new HttpRequestMessage(method, url)
        {
            Content = content
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Failed to load page {url}. Error code: {response.StatusCode}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<IReadOnlyList<string?>> LoadPagesAsync(
        IEnumerable<(string url, HttpContent httpContent)> requests,
        HttpMethod method)
    {
        var tasks = requests.Select(async request =>
        {
            await _semaphore.WaitAsync();

            try
            {
                return await LoadPageAsync(request.url, method, request.httpContent);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                _semaphore.Release();
            }
        });

        return await Task.WhenAll(tasks);
    }
}