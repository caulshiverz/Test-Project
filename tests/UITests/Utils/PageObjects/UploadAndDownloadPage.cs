using Microsoft.Playwright;

namespace UITests.Utils.PageObjects;

public class UploadAndDownloadPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/upload-download";
    public string ExpectedTitle { get; } = "Upload and Download";

    public ILocator Title => Page!.Locator("xpath=//h1[text()='Upload and Download']");
    public ILocator DownloadButton => Page!.Locator("#downloadButton");

    public async Task<UploadAndDownloadPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }
}