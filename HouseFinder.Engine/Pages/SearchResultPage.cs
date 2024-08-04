namespace HouseFinder.Engine.Pages;

public class SearchResultPage : OnTheMarketPage
{
    public SearchResultPage(IPage page) : base(page)
    {        
    }

    public ILocator BedsButton => Page.GetByRole(AriaRole.Button)
                                    .GetByText("Beds");

    public ILocator PriceButton => Page.GetByRole(AriaRole.Button)
                                    .GetByText("Price");

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
