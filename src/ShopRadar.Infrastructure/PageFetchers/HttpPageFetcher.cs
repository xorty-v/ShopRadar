using System.Text;

namespace ShopRadar.Infrastructure.PageFetchers;

public class HttpPageFetcher : IPageFetcher
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpPageFetcher(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<string>> FetchPagesAsync(List<FetchRequest> requests, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("TestClient");
        var semaphore = new SemaphoreSlim(5);

        var tasks = requests.Select(async request =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                return await FetchRequestAsync(client, request, cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        });

        var results = await Task.WhenAll(tasks);
        return results.ToList();
    }

    private async Task<string> FetchRequestAsync(
        HttpClient client,
        FetchRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            HttpResponseMessage response;

            if (request.Method == HttpMethod.Get)
            {
                response = await client.GetAsync(request.Url, cancellationToken);
            }
            else if (request.Method == HttpMethod.Post)
            {
                response = await client.PostAsync(
                    request.Url,
                    request.Body != null
                        ? new StringContent(request.GetBodyAsString()!, Encoding.UTF8, "application/json")
                        : null,
                    cancellationToken);
            }
            else
            {
                throw new NotSupportedException($"HTTP method {request.Method} is not supported.");
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            //TODO: Добавить логгер - $"Ошибка при загрузке {url}: {ex.Message}"
            return null;
        }
    }
}