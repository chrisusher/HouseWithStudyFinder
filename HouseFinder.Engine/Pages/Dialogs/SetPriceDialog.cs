namespace HouseFinder.Engine.Pages.Dialogs;

public class SetPriceDialog : OnTheMarketDialog
{
    public SetPriceDialog(IPage page) : base(page)
    {
    }

    public By ApplyPriceButton => By.Id("apply-price-btn");

    public By MinPriceSelector => By.Id("min-price");

    public By MaxPriceSelector => By.Id("max-price");
}
