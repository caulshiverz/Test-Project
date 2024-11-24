using System.Net;
using ApiTests.NUnitTests.TestFixtures;
using ApiTests.Utils.Helpers;

namespace ApiTests.NUnitTests.Endpoints.Books;

[TestFixture]
public class DeleteBookTests : GlobalSetUp
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
    }

    [Test]
    public async Task DeleteBook_ShouldReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBookDto();
        await MongoDbFixture.Books.InsertItem(book);

        //Act
        await Task.Delay(500);
        var response = await LibraryHttpService.DeleteBook(book.Title, book.Author);
        await Task.Delay(500);
        var bookDto = await MongoDbFixture.Books.GetItem(b => b.Title == book.Title && b.Author == book.Author);

        //Assert
        NUnit.Framework.Assert.Multiple(() =>
        {
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            NUnit.Framework.Assert.That(bookDto, Is.Null);
        });
    }

    [Test]
    public async Task DeleteBook_NotExistingBook_ShouldReturnNotFound()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var httpResponseMessage = await LibraryHttpService.DeleteBook(book.Title, book.Author);

        //Assert        
        NUnit.Framework.Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}