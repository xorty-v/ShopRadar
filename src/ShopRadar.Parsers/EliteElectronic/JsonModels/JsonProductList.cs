using System.Text.Json.Serialization;

namespace ShopRadar.Parsers.EliteElectronic.JsonModels;

public class JsonProductList
{
    [JsonPropertyName("total_count")] public int TotalCount { get; set; }

    [JsonPropertyName("data")] public List<JsonProduct>? Products { get; set; }
}