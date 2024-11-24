using System.Net;
using Library.Contracts.Domain;
using Library.Contracts.Mappings;
using ApiTests.Utils.Helpers;
using ApiTests.xUnitTests.Fixtures;
using Newtonsoft.Json;

namespace ApiTests.xUnitTests.Endpoints.Users;

[Collection("GlobalSetUp")]
public class CreateUsers
{
    private readonly GlobalSetUpFixture _globalSetUpFixture;

    public CreateUsers(GlobalSetUpFixture globalSetUpFixture)
    {
        _globalSetUpFixture = globalSetUpFixture;
    }

    [Fact]
    public async Task Post_NewUser_ReturnsCreated()
    {
        // Arrange
        var userToCreate = DataHelper.CreateUser();
        var response = await _globalSetUpFixture.LibraryHttpService.CreateUser(userToCreate);

        // Act
        var jsonSting = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<User>(jsonSting);
        var userDto = await _globalSetUpFixture.MongoDbFixture.Users.GetItem(u => u.NickName == user.NickName);

        // Assert
        Xunit.Assert.Multiple(() =>
        {
            Xunit.Assert.True(response.StatusCode == HttpStatusCode.Created);
            Xunit.Assert.True(user.NickName == userToCreate.NickName);
            Xunit.Assert.True(user.FullName == userToCreate.FullName);
            Xunit.Assert.NotNull(userDto);
            Xunit.Assert.True(userDto.NickName == userToCreate.NickName);
        });
    }

    [Fact]
    public async Task Post_ExistingUser_ReturnsBadRequest()
    {
        // Arrange
        var userDto = DataHelper.CreateUserDto();
        await _globalSetUpFixture.MongoDbFixture.Users.InsertItem(userDto);

        // Act
        var response = await _globalSetUpFixture.LibraryHttpService.CreateUser(userDto.ToDomain());
        var jsonSting = await response.Content.ReadAsStringAsync();
        var stringToAssert = jsonSting.Trim('"');

        // Assert
        Xunit.Assert.Multiple(() =>
        {
            Xunit.Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Xunit.Assert.Equal(stringToAssert, $"User with nickname {userDto.ToDomain().NickName} already exists");
        });
    }
}