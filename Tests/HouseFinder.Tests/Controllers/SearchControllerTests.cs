using HouseFinder.Engine.Controllers;
using HouseFinder.Engine.Pages;
using HouseFinder.Engine.Shared;
using TestingSupport.PlaywrightCommon.Helpers;
using TestingSupport.PlaywrightCommon.Shared;

namespace HouseFinder.Tests.Controllers;

[TestFixture]
public class SearchControllerTests
{
    private IPage _page;
    private SearchRequest _searchRequest;
    private SearchController _searchController;
    private string _area;

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
        };

        _searchController = new SearchController(_page, _searchRequest);
    }

    [SetUp]
    public async Task SetupEachTest()
    {
        await _searchController.PerformSearchAsync();
    }

    [Test]
    public void PerformSearch_PerformsCorrectSearch()
    {
        var resultPage = new SearchResultPage(_page);
        
        Assert.That(resultPage.OnPage, Is.True, "Page should be on result page");
        Assert.That(_page.Url.Contains(_area.ToLower()), "Page should contain searched area");
    }

    [Test]
    public async Task SetRadius_SetsCorrectRadius()
    {
        await _searchController.SetRadiusAsync();
        
        Assert.That(_page.Url.Contains("radius=2.0"), "Page should contain radius=2");
    }

    [Test]
    public async Task SetPrice_SetsCorrectPrice()
    {
        await _searchController.SetPriceAsync();

        Assert.That(_page.Url.Contains($"min-price={_searchRequest.MinimumPrice}"), "Page should contain min-price to be set to the value of the search request.");
        Assert.That(_page.Url.Contains($"max-price={_searchRequest.MaximumPrice}"), "Page should contain max-price to be set to the value of the search request.");
    }
}
