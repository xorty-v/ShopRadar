namespace ShopRadar.Infrastructure.PageFetchers;

public interface IPageFetcher
{
    public Task<List<string>> FetchPagesAsync(List<FetchRequest> requests, CancellationToken cancel = default);
}