using System.Net;
using Library.Contracts.Domain;
using Library.Contracts.Mappings;
using ApiTests.NUnitTests.TestFixtures;
using ApiTests.Utils.Helpers;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ApiTests.NUnitTests.Endpoints.Books;

[TestFixture]
public class CreateBookTests : GlobalSetUp
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
    }

    [Test]
    public async Task PostBook_ShouldReturnCreated()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var response = await LibraryHttpService.PostBook(book);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var createdBook = JsonConvert.DeserializeObject<Book>(bookJsonString);
        var bookDto = await MongoDbFixture.Books.GetItem(b => b.Title == book.Title && b.Author == book.Author && b.YearOfRelease == book.YearOfRelease);

        //Assert
        NUnit.Framework.Assert.Multiple(() =>
        {
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            NUnit.Framework.Assert.That(createdBook.Title, Is.EqualTo(book.Title));
            NUnit.Framework.Assert.That(createdBook.Author, Is.EqualTo(book.Author));
            NUnit.Framework.Assert.That(createdBook.YearOfRelease, Is.EqualTo(book.YearOfRelease));
            NUnit.Framework.Assert.That(bookDto, Is.Not.Null);
            NUnit.Framework.Assert.That(bookDto.Title, Is.EqualTo(book.Title));
            NUnit.Framework.Assert.That(bookDto.Author, Is.EqualTo(book.Author));
            NUnit.Framework.Assert.That(bookDto.YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
    public async Task PostBook_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        var bookDto = DataHelper.CreateBookDto();
        await MongoDbFixture.Books.InsertItem(bookDto);

        //Act
        var response = await LibraryHttpService.PostBook(bookDto.ToDomain());

        //Assert
        NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PostBook_ShouldReturnUnauthorized()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Arrange
        var httpResponseMessage = await LibraryHttpService.PostBook(Guid.NewGuid().ToString(), book);
        var bookDto = await MongoDbFixture.Books.GetItem(b => b.Title == book.Title && b.Author == book.Author && b.YearOfRelease == book.YearOfRelease);

        //Assert
        NUnit.Framework.Assert.Multiple(() =>
        {
            NUnit.Framework.Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            NUnit.Framework.Assert.That(bookDto, Is.Null);
        });
    }
}