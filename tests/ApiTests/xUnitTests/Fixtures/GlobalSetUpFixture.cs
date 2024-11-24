using ApiTests.Utils.Fixtures.Database;
using ApiTests.Utils.Fixtures.Docker;
using ApiTests.Utils.Fixtures.Http;
using ApiTests.Utils.Fixtures.WebApp;

namespace ApiTests.xUnitTests.Fixtures;

public class GlobalSetUpFixture : IAsyncLifetime
{
    private LibraryWebAppFixture _libraryWebAppFixture;
    private DockerFixture _dockerFixture;
    public MongoDbFixture MongoDbFixture;
    public LibraryHttpService LibraryHttpService;

    public async Task InitializeAsync()
    {
        //Starting MongoDb in Docker
        _dockerFixture = new DockerFixture();
        await _dockerFixture.StartMongo();

        //Creating an instance of MongoDbFixture
        var connectionString = _dockerFixture.GetMongoDbConnectionString();
        MongoDbFixture = new MongoDbFixture(connectionString, "LibraryV4");

        //Creating an instance of LibraryWebAppFixture
        _libraryWebAppFixture = new LibraryWebAppFixture(connectionString);

        //Creating an instance of LibraryV4 Service
        var httpClient = _libraryWebAppFixture.CreateClient();
        LibraryHttpService = new LibraryHttpService(httpClient);

        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.AuthorizeLikeDefaultUser();
    }

    public async Task DisposeAsync()
    {
        await _dockerFixture.StopMongo();
    }
}

[CollectionDefinition("GlobalSetUp")]
public class GlobalSetUpCollection : ICollectionFixture<GlobalSetUpFixture>
{

}