using System.Net;
using Library.Contracts.Mappings;
using ApiTests.Utils.Helpers;
using ApiTests.xUnitTests.Fixtures;

namespace ApiTests.xUnitTests.Endpoints.Users;

[Collection("GlobalSetUp")]
public class LoginUser
{
    private readonly GlobalSetUpFixture _globalSetUpFixture;

    public LoginUser(GlobalSetUpFixture globalSetUpFixture)
    {
        _globalSetUpFixture = globalSetUpFixture;
    }

    [Fact]
    public async Task Login_ShouldReturnOK()
    {

        //Arrange
        var userDto = DataHelper.CreateUserDto();
        await _globalSetUpFixture.MongoDbFixture.Users.InsertItem(userDto);

        //Act
        var httpResponseMessage = await _globalSetUpFixture.LibraryHttpService.LogIn(userDto.ToDomain());

        //Assert
        Xunit.Assert.True(httpResponseMessage.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_UserDoesNotExist_ShouldReturnBadRequest()
    {
        //Arrange
        var user = DataHelper.CreateUser();

        //Act
        var httpResponseMessage = await _globalSetUpFixture.LibraryHttpService.LogIn(user);

        //Assert
        Xunit.Assert.True(httpResponseMessage.StatusCode == HttpStatusCode.BadRequest);
    }
}