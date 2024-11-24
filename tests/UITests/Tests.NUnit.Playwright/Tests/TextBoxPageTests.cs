using NUnit.Framework.Interfaces;
using UITests.Utils.Fixtures;
using UITests.Utils.PageObjects;

namespace UITests.Tests.NUnit.Playwright.Tests;

[TestFixture]
public class TextBoxPageTests
{
    private readonly BrowserSetUpBuilder _browserSetUpBuilder = new();
    private TextBoxPage Page { get; set; }

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
            .OpenNewPage<TextBoxPage>();
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
    public async Task OpenTextBoxPage()
    {
        var title = await Page.Title.TextContentAsync();

        Assert.That(title, Is.EqualTo(Page.ExpectedTitle));
    }

    [Test]
    public async Task FillOutAndSubmitForm()
    {
        await Page.EnterUserName();
        await Page.EnterEmail();
        await Page.EnterCurrentAddress();
        await Page.EnterPermanentAddress();
        await Page.ClickSubmitButton();

        var output = await Page.Output.TextContentAsync();

        Assert.Multiple(() =>
        {
            Assert.That(output, Is.Not.Null);
            Assert.That(output, Is.Not.Empty);
            Assert.That(output, Does.Contain(Page.userNameText));
            Assert.That(output, Does.Contain(Page.emailText));
            Assert.That(output, Does.Contain(Page.currentAddressText));
            Assert.That(output, Does.Contain(Page.permanentAddressText));
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