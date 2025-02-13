namespace HouseFinder.Engine.Pages.Dialogs;

public class MoreFiltersDialog : OnTheMarketDialog
{
    public MoreFiltersDialog(IPage page) : base(page)
    {
    }

    public ILocator ApplyButton => Page.GetByRole(AriaRole.Button)
                                    .GetByText("Show", new()
                                    {
                                        Exact = false
                                    });

    public ILocator MinBedsSelector => Root.Locator("#min-bedrooms");
}
