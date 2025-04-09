namespace ShopRadar.Parsers.Zoommer;

public sealed record ZoommerSettings
{
    public static string CategoryUrl { get; } = "https://api.zoommer.ge/v1/Categories/all-categories";
    public static string ProductUrl { get; } = "https://api.zoommer.ge/v1/Products/v3?CategoryId=";
}