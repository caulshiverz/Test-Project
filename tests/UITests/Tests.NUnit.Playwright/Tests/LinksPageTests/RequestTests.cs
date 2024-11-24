using UITests.Utils.Fixtures;
using UITests.Utils.PageObjects;

namespace UITests.Tests.NUnit.Playwright.Tests.LinksPageTests;

[TestFixture]
public class RequestTests
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
            .WithSlowMo(500)
            .WithArgs("--start-maximized")
            .OpenNewPage<LinksPage>();
        _browserSetUpBuilder.AddRequestResponseLogger();
        await Page.Open();
    }

    [Test]
    public async Task ClickOnCreatedLink_ReturnsCorrectResponse()
    {
        await Page.CLickAndWaitForRequest(async () =>
        {
            await Page.CreatedLink.ClickAsync();
        },
        r => r.Url.Equals("https://demoqa.com/created") && r.Status == 201);

        var responseMessage = await Page.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage, Is.EqualTo("Link has responded with staus 201 and status text Created"));
    }

    [Test]
    public async Task ClickOnNoContentLink_ReturnsCorrectResponse()
    {
        await Page.CLickAndWaitForRequest(async () =>
        {
            await Page.NoContentLink.ClickAsync();
        },
        r => r.Url.Equals("https://demoqa.com/no-content") && r.Status == 204);

        var responseMessage = await Page.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage, Is.EqualTo("Link has responded with staus 204 and status text No Content"));
    }

    [Test]
    public async Task ClickOnMovedLink_ReturnsCorrectResponse()
    {
        await Page.CLickAndWaitForRequest(async () =>
        {
            await Page.MovedLink.ClickAsync();
        },
        r => r.Url.Equals("https://demoqa.com/moved") && r.Status == 301);

        var responseMessage = await Page.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage, Is.EqualTo("Link has responded with staus 301 and status text Moved Permanently"));
    }

    [Test]
    public async Task ClickOnBadRequestLink_ReturnsCorrectResponse()
    {
        await Page.CLickAndWaitForRequest(async () =>
            {
                await Page.BadRequestLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/bad-request") && r.Status == 400);

        var responseMessage = await Page.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage, Is.EqualTo("Link has responded with staus 400 and status text Bad Request"));
    }

    [Test]
    public async Task UnauthorizedLink_ReturnsCorrectResponse()
    {
        await Page.CLickAndWaitForRequest(async () =>
            {
                await Page.UnauthorizedLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/unauthorized") && r.Status == 401);

        var responseMessage = await Page.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage, Is.EqualTo("Link has responded with staus 401 and status text Unauthorized"));
    }

    [Test]
    public async Task ClickOnForbiddenLink_ReturnsCorrectResponse()
    {
        await Page.CLickAndWaitForRequest(async () =>
            {
                await Page.ForbiddenLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/forbidden") && r.Status == 403);

        var responseMessage = await Page.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage, Is.EqualTo("Link has responded with staus 403 and status text Forbidden"));
    }

    [Test]
    public async Task ClickOnNotFoundLink_ReturnsCorrectResponse()
    {
        await Page.CLickAndWaitForRequest(async () =>
            {
                await Page.NotFoundLink.ClickAsync();
            },
            r => r.Url.Equals("https://demoqa.com/invalid-url") && r.Status == 404);

        var responseMessage = await Page.ResponseMessage.TextContentAsync();

        Assert.That(responseMessage, Is.EqualTo("Link has responded with staus 404 and status text Not Found"));
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _browserSetUpBuilder.Context!.CloseAsync();
        await Page.ClosePage();
    }
}