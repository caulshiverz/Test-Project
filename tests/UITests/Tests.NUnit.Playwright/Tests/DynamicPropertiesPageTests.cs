using Microsoft.Playwright;
using NUnit.Framework.Interfaces;
using UITests.Utils.Fixtures;
using UITests.Utils.PageObjects;

namespace UITests.Tests.NUnit.Playwright.Tests;

[TestFixture]
public class DynamicPropertiesPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private DynamicPropertiesPage? Page { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Page = await _browserSetUpBuilder
            .WithBrowser(Utils.Fixtures.BrowserType.Chromium)
            .WithChannel("chrome")
            .InHeadlessMode(false)
            .WithSlowMo(100)
            .WithTimeout(10000)
            .WithVideoSize(1900, 1080)
            .SaveVideo("videos/")
            .WithArgs("--start-maximized")
            .OpenNewPage<DynamicPropertiesPage>();
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
    public async Task OpenDynamicPropertiesPage()
    {
        var title = await Page!.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo(Page.ExpectedTitle));
    }

    [Test]
    public async Task EnabledAfterButton()
    {
        await Assertions.Expect(Page!.EnabledAfterButton).ToBeEnabledAsync(new() { Timeout = 5000 });
    }

    [Test]
    public async Task ColorChangeButton()
    {
        await Assertions.Expect(Page!.ColorButton).ToHaveCSSAsync("color", "rgb(255, 255, 255)");
        await Assertions.Expect(Page!.ColorButton).ToHaveCSSAsync("color", "rgb(220, 53, 69)");
    }

    [Test]
    public async Task VisibleAfterButton()
    {
        await Assertions.Expect(Page!.VisibleAfterButton).ToBeVisibleAsync(new() { Timeout = 5000 });
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