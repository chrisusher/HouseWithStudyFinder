
namespace HouseFinder.Engine.Pages.Dialogs;

public class OnTheMarketDialog
{
    public OnTheMarketDialog(IPage page)
    {
        Page = page;
    }

    protected IPage Page { get; }

    protected ILocator Root => Page.Locator("#headlessui-portal-root");
}
