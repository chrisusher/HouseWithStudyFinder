namespace HouseFinder.Engine.Shared;

public class Property
{
    public string Heading { get; set; } = string.Empty;

    public string SubHeading { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int NumberOfBedrooms { get; set; }

    public int ListPrice { get; set; }

    public List<string> Tags { get; set; } = new();
}
