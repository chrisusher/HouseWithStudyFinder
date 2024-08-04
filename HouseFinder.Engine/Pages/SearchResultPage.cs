using HouseFinder.Engine.Pages.Dialogs;

namespace HouseFinder.Engine.Pages;

public class SearchResultPage : OnTheMarketPage
{
    public SearchResultPage(IPage page) : base(page)
    {        
    }

    public ILocator MoreFiltersButton => Page.GetByLabel("change more filters");

    public MoreFiltersDialog MoreFiltersDialog => new(Page);

    public ILocator PriceButton => Page.GetByLabel("Price");

    public SetPriceDialog PriceDialog => new(Page);

    public ILocator RadiusButton => Page.GetByLabel("Radius");

    public override bool OnPage()
    {
        if(Page.Url.Contains("/for-sale/property/"))
        {
            return true;
        }

        return false;
    }
}
