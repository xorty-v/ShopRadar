namespace ShopRadar.Application.Abstractions.Loaders;

public interface IBrowserPageLoader
{
    public Task<string> LoadPageAsync(string url, List<PageAction>? pageActions = null, bool headless = true);
    public Task<IReadOnlyList<string?>> LoadPagesAsync(IEnumerable<string> urls, List<PageAction>? pageActions = null);
}