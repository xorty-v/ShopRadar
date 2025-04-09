using System.Text.Json.Serialization;

namespace ShopRadar.Parsers.Zoommer.JsonModels;

public class JsonProduct
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("price")] public decimal ActualPrice { get; set; }

    [JsonPropertyName("previousPrice")] public decimal? PreviousPrice { get; set; }
}