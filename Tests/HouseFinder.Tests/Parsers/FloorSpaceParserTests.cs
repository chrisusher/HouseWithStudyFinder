using HouseFinder.Engine.Parsers;

namespace HouseFinder.Tests.Parsers;

[TestFixture]
public class FloorSpaceParserTests
{
    [TestCase("", null)]
    [TestCase("1,593 sq ft / 148 sq m", 1593)]
    [TestCase("1,593 sq ft", 1593)]
    [Test]
    public void Parse_ReturnsCorrectValue(string floorSpace, int? expectedValue)
    {
        var result = FloorSpaceParser.Parse(floorSpace);

        Assert.That(result, Is.EqualTo(expectedValue), $"{floorSpace} should return {expectedValue}");
    }     
}
