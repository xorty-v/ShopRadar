namespace ShopRadar.Parsers.EliteElectronic;

public sealed record EliteElectronicSettings
{
    public static string BaseUrl { get; } = "https://ee.ge/";
    public static string ApiUrl { get; } = "https://api.ee.ge/product/filter_products";
}