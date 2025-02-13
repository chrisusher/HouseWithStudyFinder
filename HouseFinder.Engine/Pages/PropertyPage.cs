namespace HouseFinder.Engine.Pages;

public class PropertyPage : OnTheMarketPage
{
    public PropertyPage(IPage page) : base(page)
    {
    }

    public ILocator MetadataLocator => Page.Locator(".mr-6");

    public string MainSectionLocator => ".main-col";

    public ILocator HeadingLocator => Page.Locator("[data-test=\"property-title\"]");

    public ILocator SubHeadingLocator => HeadingLocator.Locator("..")
                                            .Locator("//div[contains(@class, \"md:leading-normal\")]");

    public ILocator PriceLocator => Page.Locator("[data-test=\"property-price\"]");

    public ILocator TagLocator => Page.Locator(".qlVuSS")
                                    .Locator(".items-center");
}
