using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace ShopRadar.Application.Abstractions.Parsers;

[JsonConverter(typeof(StringEnumConverter))]
public enum StoreType
{
    EliteElectronic,
    Zoommer
}