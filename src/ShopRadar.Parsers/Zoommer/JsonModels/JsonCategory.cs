using System.Text.Json.Serialization;

namespace ShopRadar.Parsers.Zoommer.JsonModels;

public class JsonCategory
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }
}