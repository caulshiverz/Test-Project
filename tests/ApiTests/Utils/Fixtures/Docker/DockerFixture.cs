using DotNet.Testcontainers.Builders;
using Testcontainers.MongoDb;

namespace ApiTests.Utils.Fixtures.Docker;

public sealed class DockerFixture
{
    private readonly MongoDbContainer MongoDbContainer =
        new MongoDbBuilder()
            // .WithPortBinding(27017, 27017)
            // .WithName(Guid.NewGuid().ToString())
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017))
            .Build();

    public string GetMongoDbConnectionString() => MongoDbContainer.GetConnectionString();

    public async Task StartMongo()
    {
        await MongoDbContainer.StartAsync();
    }

    public async Task StopMongo()
    {
        await MongoDbContainer.StopAsync();
        await MongoDbContainer.DisposeAsync();
    }
}