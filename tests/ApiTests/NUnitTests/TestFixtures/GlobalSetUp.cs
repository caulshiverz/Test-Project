using ApiTests.Utils.Fixtures.Database;
using ApiTests.Utils.Fixtures.Docker;
using ApiTests.Utils.Fixtures.Http;
using ApiTests.Utils.Fixtures.WebApp;

namespace ApiTests.NUnitTests.TestFixtures;

[SetUpFixture]
public class GlobalSetUp
{
    private readonly DockerFixture _dockerFixture = new();
    private LibraryWebAppFixture _libraryWebAppFixture;
    protected MongoDbFixture MongoDbFixture;
    protected LibraryHttpService LibraryHttpService;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        //Starting MongoDb in Docker
        await _dockerFixture.StartMongo();
        var connectionString = _dockerFixture.GetMongoDbConnectionString();

        //Creating an instance of LibraryV4 Service 
        _libraryWebAppFixture = new LibraryWebAppFixture(connectionString);

        //Creating an instance of MongoDbFixture
        MongoDbFixture = new MongoDbFixture(connectionString, "LibraryV4");

        //Creating an HttpClient instance
        var httpClient = _libraryWebAppFixture.CreateClient();
        LibraryHttpService = new LibraryHttpService(httpClient);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _dockerFixture.StopMongo();
    }
}