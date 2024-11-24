using Microsoft.Playwright;

namespace UITests.Utils.PageObjects;

public class BrokenLinksAndImagesPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/broken";
    public string ExpectedTitle { get; } = "Broken Links - Images";

    public ILocator Title => Page!.Locator("//h1[text()='Broken Links - Images']");
    public ILocator ValidImage => Page!.Locator("p~img[src='/images/Toolsqa.jpg']");
    public ILocator InvalidImage => Page!.Locator("p~img[src='/images/Toolsqa_1.jpg']");
    public ILocator ValidLink => Page!.Locator("[href='http://demoqa.com']");
    public ILocator BrokenLink => Page!.Locator("[href='http://the-internet.herokuapp.com/status_codes/500']");

    public async Task<BrokenLinksAndImagesPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
}