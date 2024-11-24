using Microsoft.Playwright;

namespace UITests.Utils.PageObjects;

public class TextBoxPage : IBasePage
{
    public IPage? Page { get; set; }
    public string Url { get; } = "https://demoqa.com/text-box";
    public string ExpectedTitle { get; } = "Text Box";

    public ILocator Title => Page!.Locator("xpath=//h1[text()='Text Box']");
    public ILocator UserName => Page!.Locator("id=userName");
    public ILocator Email => Page!.Locator("id=userEmail");
    public ILocator CurrentAddress => Page!.Locator("id=currentAddress");
    public ILocator PermanentAddress => Page!.Locator("id=permanentAddress");
    public ILocator SubmitButton => Page!.Locator("id=submit");
    public ILocator Output => Page!.Locator("id=output");

    public string userNameText = "Full Name";
    public string emailText = "email@mail.com";
    public string currentAddressText = "Current Address";
    public string permanentAddressText = "Permanent Address";

    public async Task<TextBoxPage> Open()
    {
        await Page!.GotoAsync(Url);
        return this;
    }

    public async Task<TextBoxPage> EnterUserName()
    {
        await UserName.FillAsync(userNameText);
        return this;
    }

    public async Task<TextBoxPage> EnterEmail()
    {
        await Email.FillAsync(emailText);
        return this;
    }

    public async Task<TextBoxPage> EnterCurrentAddress()
    {
        await CurrentAddress.FillAsync(currentAddressText);
        return this;
    }

    public async Task<TextBoxPage> EnterPermanentAddress()
    {
        await PermanentAddress.FillAsync(permanentAddressText);
        return this;
    }

    public async Task<TextBoxPage> ClickSubmitButton()
    {
        await SubmitButton.ClickAsync();
        return this;
    }
}