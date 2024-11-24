using System.Net;
using Library.Contracts.Domain;
using Library.Contracts.Mappings;
using ApiTests.Utils.Helpers;
using ApiTests.xUnitTests.Fixtures;
using Newtonsoft.Json;

namespace ApiTests.xUnitTests.Endpoints.Books;

[Collection("GlobalSetUp")]
public class CreateBookTests
{
    private readonly GlobalSetUpFixture _globalSetUpFixture;

    public CreateBookTests(GlobalSetUpFixture globalSetUpFixture)
    {
        _globalSetUpFixture = globalSetUpFixture;
    }

    [Fact]
    public async Task PostBook_ShouldReturnCreated()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var response = await _globalSetUpFixture.LibraryHttpService.PostBook(book);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var createdBook = JsonConvert.DeserializeObject<Book>(bookJsonString);
        var bookDto = await _globalSetUpFixture.MongoDbFixture.Books.GetItem(b => b.Title == book.Title && b.Author == book.Author && b.YearOfRelease == book.YearOfRelease);

        //Assert
        Xunit.Assert.Multiple(() =>
        {
            Xunit.Assert.True(response.StatusCode == HttpStatusCode.Created);
            Xunit.Assert.True(createdBook.Title == book.Title);
            Xunit.Assert.True(createdBook.Author == book.Author);
            Xunit.Assert.True(createdBook.YearOfRelease == book.YearOfRelease);
            Xunit.Assert.NotNull(bookDto);
            Xunit.Assert.True(bookDto.Title == book.Title);
            Xunit.Assert.True(bookDto.Author == book.Author);
            Xunit.Assert.True(bookDto.YearOfRelease == book.YearOfRelease);
        });
    }

    [Fact]
    public async Task PostBook_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        var bookDto = DataHelper.CreateBookDto();
        await _globalSetUpFixture.MongoDbFixture.Books.InsertItem(bookDto);

        //Act
        var response = await _globalSetUpFixture.LibraryHttpService.PostBook(bookDto.ToDomain());

        //Assert
        Xunit.Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostBook_ShouldReturnUnauthorized()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Arrange
        var httpResponseMessage = await _globalSetUpFixture.LibraryHttpService.PostBook(Guid.NewGuid().ToString(), book);
        var bookDto = await _globalSetUpFixture.MongoDbFixture.Books.GetItem(b => b.Title == book.Title && b.Author == book.Author && b.YearOfRelease == book.YearOfRelease);

        //Assert
        Xunit.Assert.Multiple(() =>
        {
            Xunit.Assert.True(httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized);
            Xunit.Assert.Null(bookDto);
        });
    }
}