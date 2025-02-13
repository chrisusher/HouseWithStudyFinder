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
            DefaultTimeoutMs = 10000
        });

        await _page.Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        _searchRequest = new SearchRequest
        {
            Area = _area,
            MinimumPrice = 200_000,
            MaximumPrice = 450_000,
            NumberOfBedrooms = 3
        };

        _searchController = new SearchController(_page, _searchRequest);
    }

    [OneTimeTearDown]
    public async Task ClassTearDown()
    {
        await _page.Context.Tracing.StopAsync(new TracingStopOptions
        {
            Path = $"{LoggingHelper.LogDirectory}/trace.zip"
        });
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
        Assert.That(_page.Url.Contains(_area, StringComparison.CurrentCultureIgnoreCase), "Page should contain searched area");
    }

    [Test]
    public async Task SetBeds_SetsCorrectBeds()
    {
        await _searchController.SetBedsAsync();

        // Shouldn't be required but the url does not change until a delay.
        await Task.Delay(3000);

        Assert.That(_searchController.Page.Url, Does.Contain($"min-bedrooms={_searchRequest.NumberOfBedrooms}"), "Page should contain min-bedrooms to be set to the value of the search request.");
    }

    [Test]
    public async Task SetRadius_SetsCorrectRadius()
    {
        await _searchController.SetRadiusAsync();
        
        Assert.That(_searchController.Page.Url, Does.Contain("radius=2.0"), "Page should contain radius=2");
    }

    [Test]
    public async Task SetPrice_SetsCorrectPrice()
    {
        await _searchController.SetPriceAsync();

        var url = _searchController.Page.Url;

        Assert.That(url, Does.Contain($"min-price={_searchRequest.MinimumPrice}"), "Page should contain min-price to be set to the value of the search request.");
        Assert.That(url, Does.Contain($"max-price={_searchRequest.MaximumPrice}"), "Page should contain max-price to be set to the value of the search request.");
    }
}
