using HouseFinder.Engine.Pages;
using TestingSupport.PlaywrightCommon.Helpers;
using TestingSupport.PlaywrightCommon.Shared;

namespace HouseFinder.Tests.Pages;

public class SearchResultPageTests
{
    private string _url;
    private IPage _page;
    private PropertyPage _propertyPage;

    [OneTimeSetUp]
    public async Task ClassSetup()
    {
        _url = "https://www.onthemarket.com/details/14816491/";
        _page = await BrowserManager.LaunchBrowserAsync(new BrowserConfig
        {
            Headless = false,
            DefaultTimeoutMs = 30000
        });

        await _page.GotoAsync(_url);

        _propertyPage = new PropertyPage(_page);
    }

    [Test]
    public async Task Property_HasHeading()
    {
        var heading = await _propertyPage.HeadingLocator.TextContentAsync();

        LoggingHelper.Log($"Heading: {heading}");

        Assert.That(heading, Is.Not.Null, "Heading should not be null.");
        Assert.That(heading.Contains("3 bedroom semi"), "Heading should contain '3 bedroom semi'.");
    }

    [Test]
    public async Task Property_HasSubHeading()
    {
        var heading = await _propertyPage.SubHeadingLocator.TextContentAsync();

        LoggingHelper.Log($"Heading: {heading}");

        Assert.That(heading, Is.Not.Null, "Heading should not be null.");
        Assert.That(heading.Contains("Swansea"), "Heading should contain 'Swansea'.");
    }

    [Test]
    public async Task Property_HasPrice()
    {
        var heading = await _propertyPage.PriceLocator.TextContentAsync();

        LoggingHelper.Log($"Heading: {heading}");

        Assert.That(heading, Is.Not.Null, "Heading should not be null.");
        Assert.That(heading.Contains("£"), "Heading should contain '£'.");
    }
}
