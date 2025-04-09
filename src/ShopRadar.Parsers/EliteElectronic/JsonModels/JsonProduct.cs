using System.Text.Json.Serialization;

namespace ShopRadar.Parsers.EliteElectronic.JsonModels;

public class JsonProduct
{
    [JsonPropertyName("product_desc")] public string Name { get; set; }

    [JsonPropertyName("actual_price")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal ActualPrice { get; set; }

    [JsonPropertyName("sale_price")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal SalePrice { get; set; }
}