namespace HouseFinder.Engine.Pages;

public class PropertyPage : OnTheMarketPage
{
    public PropertyPage(IPage page) : base(page)
    {
    }

    public ILocator MetadataLocator => Page.Locator(".mr-6");

    public string MainSectionLocator => ".main-col";

    public ILocator HeadingLocator => Page.Locator($"{MainSectionLocator} {By.TagName("h1").Locator}");

    public ILocator SubHeadingLocator => Page.Locator($"{MainSectionLocator} .leading-none");

    public ILocator PriceLocator => Page.Locator($"{MainSectionLocator} .price");

    public ILocator TagLocator => Page.Locator($"{MainSectionLocator} .otm-LabelChip");
}
