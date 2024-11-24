using NUnit.Framework.Interfaces;
using UITests.Utils.Fixtures;
using UITests.Utils.PageObjects;

namespace UITests.Tests.NUnit.Playwright.Tests;

[TestFixture]
public class BrokenLinksAndImagesPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private BrokenLinksAndImagesPage? Page { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(false)
            .WithSlowMo(100)
            .WithTimeout(10000)
            .WithVideoSize(1900, 1080)
            .SaveVideo("videos/")
            .WithArgs("--start-maximized")
            .OpenNewPage<BrokenLinksAndImagesPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }

    [SetUp]
    public async Task SetUp()
    {
        var traceName = TestContext.CurrentContext.Test.ClassName + "/" + TestContext.CurrentContext.Test.Name;
        await _browserSetUpBuilder.StartTracing(traceName);
    }

    [Test]
    public async Task OpenBrokenLinksAndImagesPage()
    {
        var title = await Page!.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo(Page.ExpectedTitle));
    }

    [Test]
    public async Task CheckValidImage()
    {
        var isImageVisible = await Page!.ValidImage.IsVisibleAsync();

        Assert.That(isImageVisible, Is.True);
    }

    [Test]
    public async Task CheckInvalidImage()
    {
        var isImageVisible = await Page!.InvalidImage.IsVisibleAsync();

        Assert.That(isImageVisible, Is.True);
    }

    [Test]
    public async Task OpenValidLink()
    {
        await Page!.ValidLink.ClickAsync();
        var url = Page.Page!.Url;

        Assert.That(url, Is.EqualTo("https://demoqa.com/"));

        await Page.Page.GoBackAsync();
    }

    [Test]
    public async Task OpenBrokenLink()
    {
        await Page!.BrokenLink.ClickAsync();
        var url = Page.Page!.Url;

        Assert.That(url, Is.EqualTo("http://the-internet.herokuapp.com/status_codes/500"));

        await Page.Page.GoBackAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            await _browserSetUpBuilder.Screenshot(
                TestContext.CurrentContext.Test.ClassName!,
                TestContext.CurrentContext.Test.Name);
        }

        var tracePath = Path.Combine(
            "playwright-traces",
            $"{TestContext.CurrentContext.Test.ClassName}",
            $"{TestContext.CurrentContext.Test.Name}.zip");
        await _browserSetUpBuilder.StopTracing(tracePath);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Page!.CloseAsync();
        await _browserSetUpBuilder.Context!.CloseAsync();
    }
}