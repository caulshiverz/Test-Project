using System.Net;
using Library.Contracts.Domain;
using ApiTests.Utils.Helpers;
using ApiTests.xUnitTests.Fixtures;
using Newtonsoft.Json;

namespace ApiTests.xUnitTests.Endpoints.Books;

[Collection("GlobalSetUp")]
public class GetBooks
{
    private readonly GlobalSetUpFixture _globalSetUpFixture;

    public GetBooks(GlobalSetUpFixture globalSetUpFixture)
    {
        _globalSetUpFixture = globalSetUpFixture;
    }

    [Fact]
    public async Task GetBooksByTitle_ShouldReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBookDto();
        await _globalSetUpFixture.MongoDbFixture.Books.InsertItem(book);

        //Act
        var response = await _globalSetUpFixture.LibraryHttpService.GetBooksByTitle(book.Title);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        Xunit.Assert.Multiple(() =>
        {
            Xunit.Assert.True(response.StatusCode == HttpStatusCode.OK);
            Xunit.Assert.True(bookFromResponse[0].Title == book.Title);
            Xunit.Assert.True(bookFromResponse[0].Author == book.Author);
            Xunit.Assert.True(bookFromResponse[0].YearOfRelease == book.YearOfRelease);
        });
    }

    [Fact]
    public async Task GetBooksByTitle_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var response = await _globalSetUpFixture.LibraryHttpService.GetBooksByTitle(book.Title);

        //Assert
        Xunit.Assert.True(response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetBooksByAuthor_ShouldReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBookDto();
        await _globalSetUpFixture.MongoDbFixture.Books.InsertItem(book);

        //Act
        var response = await _globalSetUpFixture.LibraryHttpService.GetBooksByAuthor(book.Author);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        Xunit.Assert.Multiple(() =>
        {
            Xunit.Assert.True(response.StatusCode == HttpStatusCode.OK);
            Xunit.Assert.True(bookFromResponse[0].Title == book.Title);
            Xunit.Assert.True(bookFromResponse[0].Author == book.Author);
            Xunit.Assert.True(bookFromResponse[0].YearOfRelease == book.YearOfRelease);
        });
    }

    [Fact]
    public async Task GetBooksByAuthor_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var response = await _globalSetUpFixture.LibraryHttpService.GetBooksByAuthor(book.Author);

        //Assert
        Xunit.Assert.True(response.StatusCode == HttpStatusCode.NotFound);
    }
}