using UITests.Utils.Fixtures;
using UITests.Utils.PageObjects;

namespace UITests.Tests.NUnit.Playwright.Tests.LinksPageTests;

[TestFixture]
public class LinksThatLeadToAnotherPage
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private LinksPage Page { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(true)
            .WithTimeout(10000)
            .WithSlowMo(100)
            .WithArgs("--start-maximized")
            .OpenNewPage<LinksPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }

    [Test]
    public async Task OpenLinksPage()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo(Page.ExpectedTitle));
    }

    [Test]
    public async Task ClickHomeLink_ReturnsCorrectTab()
    {
        await Page.HomeLink.ClickAsync();
        var expectedUrl = Page.Page!.Url;

        Assert.That(expectedUrl, Is.EqualTo(Page.Url));
    }

    [Test]
    public async Task ClickHomeZDs2pLink_ReturnsCorrectTab()
    {
        await Page.HomeZDs2pLink.ClickAsync();
        var expectedUrl = Page.Page!.Url;

        Assert.That(expectedUrl, Is.EqualTo(Page.Url));
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Context!.CloseAsync();
        await Page.ClosePage();
    }
}