using System.Net;
using Library.Contracts.Domain;
using ApiTests.NUnitTests.TestFixtures;
using ApiTests.Utils.Helpers;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ApiTests.NUnitTests.Endpoints.Books;

[TestFixture]
public class GetBooks : GlobalSetUp
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
    }

    [Test]
    public async Task GetBooksByTitle_ShouldReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBookDto();
        await MongoDbFixture.Books.InsertItem(book);

        //Act
        var response = await LibraryHttpService.GetBooksByTitle(book.Title);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        NUnit.Framework.Assert.Multiple(() =>
        {
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            NUnit.Framework.Assert.That(bookFromResponse[0].Title, Is.EqualTo(book.Title));
            NUnit.Framework.Assert.That(bookFromResponse[0].Author, Is.EqualTo(book.Author));
            NUnit.Framework.Assert.That(bookFromResponse[0].YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
    public async Task GetBooksByTitle_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var response = await LibraryHttpService.GetBooksByTitle(book.Title);

        //Assert
        NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetBooksByAuthor_ShouldReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBookDto();
        await MongoDbFixture.Books.InsertItem(book);

        //Act
        var response = await LibraryHttpService.GetBooksByAuthor(book.Author);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        NUnit.Framework.Assert.Multiple(() =>
        {
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            NUnit.Framework.Assert.That(bookFromResponse[0].Title, Is.EqualTo(book.Title));
            NUnit.Framework.Assert.That(bookFromResponse[0].Author, Is.EqualTo(book.Author));
            NUnit.Framework.Assert.That(bookFromResponse[0].YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
    public async Task GetBooksByAuthor_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var response = await LibraryHttpService.GetBooksByAuthor(book.Author);

        //Assert
        NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}