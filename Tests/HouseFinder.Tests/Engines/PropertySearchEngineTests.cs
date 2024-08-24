using HouseFinder.Engine.Engines;
using HouseFinder.Engine.Shared;
using TestingSupport.PlaywrightCommon.Helpers;
using TestingSupport.PlaywrightCommon.Shared;

namespace HouseFinder.Tests.Engines;

[TestFixture]
public class PropertySearchEngineTests
{
    private string _area;
    private IPage _page;
    private SearchRequest _searchRequest;
    private PropertySearchEngine _propertySearchEngine;

    [OneTimeSetUp]
    public async Task ClassSetup()
    {
        _area = "Sketty";
        _page = await BrowserManager.LaunchBrowserAsync(new BrowserConfig
        {
            Headless = false,
            DefaultTimeoutMs = 30000
        });

        _searchRequest = new SearchRequest
        {
            Area = _area,
            MinimumPrice = 200_000,
            MaximumPrice = 450_000,
            NumberOfBedrooms = 3
        };

        _propertySearchEngine = new PropertySearchEngine(_page, _searchRequest);
    }

    [Test]
    public async Task GetAllProperties_ReturnsMoreThanPageSize()
    {
        var allProperties = await _propertySearchEngine.GetAllPropertiesAsync();

        Assert.That(allProperties.Properties.Count, Is.AtLeast(30), "GetAllPropertiesAsync() Should return at least 30 properties");
    }
}
