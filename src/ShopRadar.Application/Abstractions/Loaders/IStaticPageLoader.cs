namespace ShopRadar.Application.Abstractions.Loaders;

public interface IStaticPageLoader
{
    public Task<string> LoadPageAsync(string url, HttpMethod method, HttpContent? content);

    public Task<IReadOnlyList<string?>> LoadPagesAsync(
        IEnumerable<(string url, HttpContent httpContent)> requests,
        HttpMethod method);
}