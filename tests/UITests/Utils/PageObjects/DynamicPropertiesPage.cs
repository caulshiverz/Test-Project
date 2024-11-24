using Microsoft.Playwright;

namespace UITests.Utils.PageObjects;

public class DynamicPropertiesPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/dynamic-properties";
    public string ExpectedTitle { get; } = "Dynamic Properties";

    public ILocator Title => Page!.Locator("//h1[text()='Dynamic Properties']");
    public ILocator EnabledAfterButton => Page!.Locator("button#enableAfter");
    public ILocator ColorButton => Page!.Locator("#colorChange");
    public ILocator VisibleAfterButton => Page!.Locator("#visibleAfter");

    public async Task<DynamicPropertiesPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
}
