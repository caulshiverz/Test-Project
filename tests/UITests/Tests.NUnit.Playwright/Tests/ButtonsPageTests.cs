using NUnit.Framework.Interfaces;
using UITests.Utils.Fixtures;
using UITests.Utils.PageObjects;

namespace UITests.Tests.NUnit.Playwright.Tests;

[TestFixture]
public class ButtonsPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private ButtonsPage Page { get; set; }

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
            .OpenNewPage<ButtonsPage>();
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
    public async Task OpenButtonsPage()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo(Page.ExpectedTitle));
    }

    [Test]
    public async Task DoubleClickButtonTest()
    {
        var isVisible = await Page.DoubleClickButton.IsVisibleAsync();
        var isEnabled = await Page.DoubleClickButton.IsEnabledAsync();

        await Page.DoubleClick();

        var textOutput = await Page.DoubleClickMessage.TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.That(isVisible, Is.True);
            Assert.That(isEnabled, Is.True);
            Assert.That(textOutput, Is.EqualTo("You have done a double click"));
        });
    }

    [Test]
    public async Task RightClickButtonTest()
    {
        var isVisible = await Page.RightClickButton.IsVisibleAsync();
        var isEnabled = await Page.RightClickButton.IsEnabledAsync();

        await Page.ClickOnRightClickButton();

        var textOutput = await Page.RightClickMessage.TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.That(isVisible, Is.True);
            Assert.That(isEnabled, Is.True);
            Assert.That(textOutput, Is.EqualTo("You have done a right click"));
        });
    }

    [Test]
    public async Task ClickMeButtonTest()
    {
        var isVisible = await Page.ClickMeButton.IsVisibleAsync();
        var isEnabled = await Page.ClickMeButton.IsEnabledAsync();

        await Page.ClickOnClickMe();

        var textOutput = await Page.ClickMeMessage.TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.That(isVisible, Is.True);
            Assert.That(isEnabled, Is.True);
            Assert.That(textOutput, Is.EqualTo("You have done a dynamic click"));
        });
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