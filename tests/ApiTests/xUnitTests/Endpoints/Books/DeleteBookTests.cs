using System.Net;
using ApiTests.Utils.Helpers;
using ApiTests.xUnitTests.Fixtures;
using Xunit.Abstractions;

namespace ApiTests.xUnitTests.Endpoints.Books;

[Collection("GlobalSetUp")]
public class DeleteBookTests
{
    private readonly GlobalSetUpFixture _globalSetUpFixture;
    private readonly ITestOutputHelper _output;

    public DeleteBookTests(GlobalSetUpFixture globalSetUpFixture, ITestOutputHelper output)
    {
        _globalSetUpFixture = globalSetUpFixture;
        _output = output;
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBookDto();
        await _globalSetUpFixture.MongoDbFixture.Books.InsertItem(book);

        //Act
        await Task.Delay(500);
        var response = await _globalSetUpFixture.LibraryHttpService.DeleteBook(book.Title, book.Author);
        var bookDto = await _globalSetUpFixture.MongoDbFixture.Books.GetItem(b => b.Title == book.Title && b.Author == book.Author);
        var jsonString = await response.Content.ReadAsStringAsync();
        _output.WriteLine(jsonString);

        //Assert
        Xunit.Assert.Multiple(() =>
        {
            Xunit.Assert.True(response.StatusCode == HttpStatusCode.OK);
            Xunit.Assert.Null(bookDto);
        });
    }

    [Fact]
    public async Task DeleteBook_NotExistingBook_ShouldReturnNotFound()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var httpResponseMessage = await _globalSetUpFixture.LibraryHttpService.DeleteBook(book.Title, book.Author);

        //Assert
        Xunit.Assert.True(httpResponseMessage.StatusCode == HttpStatusCode.NotFound);
    }
}