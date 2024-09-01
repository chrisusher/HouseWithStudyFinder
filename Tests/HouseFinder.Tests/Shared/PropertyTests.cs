using HouseFinder.Engine.Shared;

namespace HouseFinder.Tests.Shared;

[TestFixture]
public class PropertyTests
{
    private Property _property;

    [OneTimeSetUp]
    public void ClassSetup()
    {
        var link = "https://www.onthemarket.com/details/14816491/";

        _property = new Property
        {
            Url = link,
        };
    }

    [Test]
    public void Property_Id_IsNotZero()
    {
        Assert.That(_property.Id, Is.Not.EqualTo(0), "Property Id should not be zero.");
    }
}