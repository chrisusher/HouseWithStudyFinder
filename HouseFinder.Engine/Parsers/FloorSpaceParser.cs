namespace HouseFinder.Engine.Parsers;

public static class FloorSpaceParser
{
    public static int? Parse(string floorSpace)
    {
        if (string.IsNullOrEmpty(floorSpace))
        {
            return null;
        }

        var stringSplit = floorSpace.Split("/");

        if (stringSplit.Length > 2 || stringSplit.Length < 1)
        {
            return null;
        }

        var firstPart = stringSplit.FirstOrDefault(x => x.Contains("sq ft", StringComparison.CurrentCultureIgnoreCase));

        if(firstPart is null)
        {
            return null;
        }

        firstPart = firstPart.ToLower();

        firstPart = firstPart.Replace(" sq ft", string.Empty);
        firstPart = firstPart.Replace(",", "");

        if (int.TryParse(firstPart, out var result))
        {
            return result;
        }

        return null;
    }
}
