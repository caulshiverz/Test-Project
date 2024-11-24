using System.Net;
using Library.Contracts.Domain;
using Library.Contracts.Mappings;
using ApiTests.NUnitTests.TestFixtures;
using ApiTests.Utils.Helpers;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace ApiTests.NUnitTests.Endpoints.Users;

[TestFixture]
public sealed class CreateUser : GlobalSetUp
{
    [Test]
    public async Task Post_NewUser_ReturnsCreated()
    {
        // Arrange
        var userToCreate = DataHelper.CreateUser();
        var response = await LibraryHttpService.CreateUser(userToCreate);

        // Act
        var jsonSting = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<User>(jsonSting);
        var userDto = await MongoDbFixture.Users.GetItem(u => u.NickName == user.NickName);

        // Assert
        NUnit.Framework.Assert.Multiple(() =>
        {
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            NUnit.Framework.Assert.That(user.NickName, Is.EqualTo(userToCreate.NickName));
            NUnit.Framework.Assert.That(user.FullName, Is.EqualTo(userToCreate.FullName));
            NUnit.Framework.Assert.That(userDto, Is.Not.Null);
            NUnit.Framework.Assert.That(userDto.NickName, Is.EqualTo(userToCreate.NickName));
        });
    }

    [Test]
    public async Task Post_ExistingUser_ReturnsBadRequest()
    {
        // Arrange
        var userDto = DataHelper.CreateUserDto();
        await MongoDbFixture.Users.InsertItem(userDto);

        // Act
        var response = await LibraryHttpService.CreateUser(userDto.ToDomain());
        var jsonSting = await response.Content.ReadAsStringAsync();
        var stringToAssert = jsonSting.Trim('"');

        // Assert
        NUnit.Framework.Assert.Multiple(() =>
        {
            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            NUnit.Framework.Assert.That(stringToAssert, Is.EqualTo($"User with nickname {userDto.ToDomain().NickName} already exists"));
        });
    }
}