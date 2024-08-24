namespace HouseFinder.Engine.Shared;

public class Property
{
    private int _numberOfBedrooms;

    public string Heading { get; set; } = string.Empty;

    public string SubHeading { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int NumberOfBedrooms
    {
        get
        {
            if (_numberOfBedrooms > 0)
            {
                return _numberOfBedrooms;
            }
            if (string.IsNullOrEmpty(Heading))
            {
                return _numberOfBedrooms;
            }

            var headingWords = Heading.Split(' ');

            if (int.TryParse(headingWords[0], out _numberOfBedrooms))
            {
                return _numberOfBedrooms;
            }

            return _numberOfBedrooms;
        }
    }

    public int ListPrice { get; set; }

    public List<string> Tags { get; set; } = new();
}
