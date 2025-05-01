using System.Text;
using System.Text.Json;

namespace ShopRadar.Parsers.EliteElectronic.Requests;

public class PostRequestBody
{
    public int min_price { get; set; } = 0;
    public int max_price { get; set; } = 0;
    public string category { get; set; }
    public string[] brand { get; set; } = [];
    public string[] color { get; set; } = [];
    public string[] room { get; set; } = [];
    public string sort_by { get; set; } = string.Empty;
    public int item_per_page { get; set; } = 10;
    public int page_no { get; set; } = 1;
    public string[] specification { get; set; } = [];
    public int sale_products { get; set; } = 0;
    public string search_text { get; set; } = string.Empty;
    public string slug { get; set; } = string.Empty;
    public int? pageno { get; set; } = null;

    public static HttpContent Create(string category, int page = 1, int itemsPerPage = 10)
    {
        var body = new PostRequestBody
        {
            category = category,
            page_no = page,
            item_per_page = itemsPerPage
        };

        var json = JsonSerializer.Serialize(body);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}