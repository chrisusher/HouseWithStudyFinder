using HouseFinder.Engine.Enums;
using HouseFinder.Engine.Pages;
using HouseFinder.Engine.Shared;

namespace HouseFinder.Engine.Controllers;

public class SearchController
{
    private readonly IPage _page;
    private readonly HomePage _homePage;
    private readonly SearchRequest _searchRequest;

    private SearchResultPage ResultPage => new(_page);
    
    public SearchController(IPage page, SearchRequest searchRequest)
    {
        _page = page;
        _searchRequest = searchRequest;

        _homePage = new HomePage(_page);
    }

    public async Task PerformSearchAsync()
    {
        if (!_homePage.OnPage())
        {
            await _homePage.NavigateAsync();
        }

        await _page.FillAsync(HomePage.SearchBox.Locator, _searchRequest.Area);

        await _page.ClickAsync(HomePage.SearchButton.Locator);
    }

    public async Task SetRadiusAsync()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await ResultPage.RadiusButton.ClickAsync();

        var radiusLocator = By.Id("radius-selector");
        
        switch (_searchRequest.Radius)
        {
            case RadiusType.TwoMiles:
                await _page.SelectOptionAsync(radiusLocator.Locator, "+2 miles");
                break;
            default:
                throw new NotImplementedException($"Radius type {_searchRequest.Radius} is not implemented");
        }

        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}