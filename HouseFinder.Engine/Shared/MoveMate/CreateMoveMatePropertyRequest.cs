using System.Text.Json.Serialization;

namespace HouseFinder.Engine.Shared.MoveMate;

public class CreateMoveMatePropertyRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("maxValue")]
    public double MaxValue { get; set; }

    [JsonPropertyName("minValue")]
    public double MinValue { get; set; }

    [JsonPropertyName("propertyType")]
    public string PropertyType { get; set; } = "ToPurchase";

    [JsonPropertyName("notes")]
    public List<string> Notes { get; set; }

    [JsonPropertyName("marketDetails")]
    public PropertyMarketDetails MarketDetails { get; set; }
}
