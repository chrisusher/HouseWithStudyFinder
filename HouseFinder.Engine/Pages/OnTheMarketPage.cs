
namespace HouseFinder.Engine.Pages;

public class OnTheMarketPage
{
    public OnTheMarketPage(IPage page)
    {
        Page = page;
    }

    protected IPage Page { get; }

    public virtual Task NavigateAsync() => Task.CompletedTask;

    public virtual bool OnPage() => false;    
}
