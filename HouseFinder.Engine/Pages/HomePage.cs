namespace HouseFinder.Engine.Pages;

public class HomePage : OnTheMarketPage
{
    public HomePage(IPage page) : base(page)
    {
    }

    public static By SearchBox => By.Id("search-location-sale");

    public static By SearchButton => By.Id("search-location-sale-btn");

    public override Task NavigateAsync()
    {
        return Page.GotoAsync("https://www.onthemarket.com/");
    }

    public override bool OnPage()
    {
        if (Page.Url == "https://www.onthemarket.com/")
        {
            return true;
        }

        return false;
    }
}