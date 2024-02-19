namespace SpendManagement.Integration.Tests.Fixtures
{
    [CollectionDefinition(nameof(SharedFixtureCollection))]
    public class SharedFixtureCollection :
         ICollectionFixture<KafkaFixture>,
         ICollectionFixture<MongoDbFixture>,
         ICollectionFixture<HttpFixture>
    {
    }
}
