using HouseFinder.Engine.Enums;
using HouseFinder.Engine.Pages;
using HouseFinder.Engine.Shared;

namespace HouseFinder.Engine.Controllers;

public class SearchController : BaseController
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

        // Accept cookies
        await _page.GetByRole(AriaRole.Button)
            .GetByText("Accept all")
            .ClickAsync();

        await _page.FillAsync(HomePage.SearchBox.Locator, _searchRequest.Area);

        await _page.ClickAsync(HomePage.SearchButton.Locator);
    }

    public async Task SetBedsAsync()
    {
        var minBedsLocator = ResultPage.MoreFiltersDialog.MinBedsSelector;

        await RetryPolicy.ExecuteAsync(async () =>
        {
            if (await _page.IsVisibleAsync(minBedsLocator.Locator))
            {
                return;
            }

            await ResultPage.MoreFiltersButton.ClickAsync();

            if (!await _page.IsVisibleAsync(minBedsLocator.Locator))
            {
                throw new Exception("Min Beds selector not visible");
            }
        });

        await _page.SelectOptionAsync(minBedsLocator.Locator, _searchRequest.NumberOfBedrooms.ToString());

        await ResultPage.MoreFiltersDialog.ApplyButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task SetRadiusAsync()
    {
        var radiusLocator = By.Id("radius-selector");

        await RetryPolicy.ExecuteAsync(async () =>
        {
            await ResultPage.RadiusButton.ClickAsync();

            if (!await _page.IsVisibleAsync(radiusLocator.Locator))
            {
                throw new Exception("Radius selector not visible");
            }
        });

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

    public async Task SetPriceAsync()
    {
        await SetMinPrice();
        await SetMaxPrice();

        await _page.ClickAsync(ResultPage.PriceDialog.ApplyPriceButton.Locator);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #region Private Methods

    private async Task SetMaxPrice()
    {
        var maxPriceLocator = ResultPage.PriceDialog.MaxPriceSelector;

        await RetryPolicy.ExecuteAsync(async () =>
        {
            if (await _page.IsVisibleAsync(maxPriceLocator.Locator))
            {
                return;
            }

            await ResultPage.PriceButton.ClickAsync();

            if (!await _page.IsVisibleAsync(maxPriceLocator.Locator))
            {
                throw new Exception("Max price selector not visible");
            }
        });

        await _page.SelectOptionAsync(maxPriceLocator.Locator, _searchRequest.MaximumPrice.ToString());
    }

    private async Task SetMinPrice()
    {
        var minPriceLocator = ResultPage.PriceDialog.MinPriceSelector;

        await RetryPolicy.ExecuteAsync(async () =>
        {
            if (await _page.IsVisibleAsync(minPriceLocator.Locator))
            {
                return;
            }

            await ResultPage.PriceButton.ClickAsync();

            if (!await _page.IsVisibleAsync(minPriceLocator.Locator))
            {
                throw new Exception("Min price selector not visible");
            }
        });

        await _page.SelectOptionAsync(minPriceLocator.Locator, _searchRequest.MinimumPrice.ToString());
    }

    #endregion Private Methods
}