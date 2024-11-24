using System.Net;
using Library.Contracts.Mappings;
using ApiTests.NUnitTests.TestFixtures;
using ApiTests.Utils.Helpers;

namespace ApiTests.NUnitTests.Endpoints.Users;

[TestFixture]
public sealed class LoginUser : GlobalSetUp
{
    [Test]
    public async Task Login_ShouldReturnOK()
    {

        //Arrange
        var userDto = DataHelper.CreateUserDto();
        await MongoDbFixture.Users.InsertItem(userDto);

        //Act
        var httpResponseMessage = await LibraryHttpService.LogIn(userDto.ToDomain());

        //Assert
        NUnit.Framework.Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Login_UserDoesNotExist_ShouldReturnBadRequest()
    {
        //Arrange
        var user = DataHelper.CreateUser();

        //Act
        var httpResponseMessage = await LibraryHttpService.LogIn(user);

        //Assert
        NUnit.Framework.Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}