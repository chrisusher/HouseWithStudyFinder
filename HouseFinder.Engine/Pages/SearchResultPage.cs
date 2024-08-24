using HouseFinder.Engine.Pages.Dialogs;

namespace HouseFinder.Engine.Pages;

public class SearchResultPage : OnTheMarketPage
{
    public SearchResultPage(IPage page) : base(page)
    {
    }

    public ILocator MoreFiltersButton => Page.GetByLabel("change more filters");

    public MoreFiltersDialog MoreFiltersDialog => new(Page);

    public ILocator NextPageButton => Page.GetByTitle("Next page");

    public ILocator PriceButton => Page.GetByLabel("Price");

    public SetPriceDialog PriceDialog => new(Page);

    public ILocator PropertyCards => Page.GetByRole(AriaRole.Article);

    public ILocator RadiusButton => Page.GetByLabel("Radius");

    private ILocator SearchResultsContainer => Page.Locator(By.Id("maincontent").Locator);

    public async Task<List<IElementHandle>> GetPropertyLocators()
    {
        return (await SearchResultsContainer.ElementHandlesAsync()).ToList();
    }

    public override bool OnPage()
    {
        if (Page.Url.Contains("/for-sale/property/"))
        {
            return true;
        }

        return false;
    }
}
