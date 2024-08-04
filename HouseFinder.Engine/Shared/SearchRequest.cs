using HouseFinder.Engine.Enums;

namespace HouseFinder.Engine.Shared;

public class SearchRequest
{
    public string Area { get; set; } = string.Empty;

    public int NumberOfBedrooms { get; set; } = 3;

    public int MinimumPrice { get; set; }

    public int MaximumPrice { get; set; }

    public RadiusType Radius { get; set; } = RadiusType.TwoMiles;
}