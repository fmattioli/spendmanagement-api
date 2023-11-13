using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using SpendManagement.Integration.Tests.Configuration;

namespace SpendManagement.Integration.Tests.Fixtures
{
    public class MongoDbFixture : IAsyncLifetime
    {
        public readonly IMongoDatabase database;
        private readonly List<Guid> categoryIds = new();

        public MongoDbFixture()
        {
            var mongoUrl = new MongoUrl(TestSettings.MongoSettings.ConnectionString);
            this.database = new MongoClient(mongoUrl).GetDatabase(TestSettings.MongoSettings.Database);
        }

        public async Task DisposeAsync()
        {
            var collection = this.database.GetCollection<Category>("Categories");

            if (categoryIds.Any())
            {
                var filter = new FilterDefinitionBuilder<Category>()
                    .In(x => x.Id, categoryIds);

                await collection.DeleteManyAsync(filter);
            }
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public void AddCategoryToCleanUp(Guid id)
        {
            this.categoryIds.Add(id);
        }

        public async Task InsertCategory(Category category)
        {
            var collection = this.database.GetCollection<Category>("Categories");
            await collection.InsertOneAsync(category);
            this.categoryIds.Add(category.Id);
        }
    }

    public class Category
    {
        [BsonId]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
