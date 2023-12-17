using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using SpendManagement.Integration.Tests.Configuration;

namespace SpendManagement.Integration.Tests.Fixtures
{
    public class MongoDbFixture : IAsyncLifetime
    {
        public readonly IMongoDatabase database;
        private readonly List<Guid> categoryIds = new();
        private readonly List<Guid> receiptIds = new();

        public MongoDbFixture()
        {
            var mongoUrl = new MongoUrl(TestSettings.MongoSettings.ConnectionString);
            this.database = new MongoClient(mongoUrl).GetDatabase(TestSettings.MongoSettings.Database);
        }

        public async Task DisposeAsync()
        {
            if (categoryIds.Any())
            {
                var collection = this.database.GetCollection<Category>("Categories");

                var filter = new FilterDefinitionBuilder<Category>()
                    .In(x => x.Id, categoryIds);

                await collection.DeleteManyAsync(filter);
            }

            if (receiptIds.Any())
            {
                var collection = this.database.GetCollection<Receipt>("Receipts");

                var filter = new FilterDefinitionBuilder<Receipt>()
                    .In(x => x.Id, receiptIds);

                await collection.DeleteManyAsync(filter);
            }
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public void AddCategoryToCleanUp(Guid id)
        {
            this.categoryIds.Add(id);
        }

        public async Task InsertReceipt(Receipt receipt)
        {
            var collection = this.database.GetCollection<Receipt>("Receipts");
            await collection.InsertOneAsync(receipt);
            this.receiptIds.Add(receipt.Id);
        }

        public async Task InsertCategory(Category category)
        {
            var collection = this.database.GetCollection<Category>("Categories");
            await collection.InsertOneAsync(category);
            this.categoryIds.Add(category.Id);
        }

        public async Task InsertCategories(IEnumerable<Category>? category)
        {
            var collection = this.database.GetCollection<Category>("Categories");
            await Task.WhenAll(category!.Select(x => collection.InsertOneAsync(x)));
            this.categoryIds.AddRange(category!.Select(x => x.Id));
        }
    }

    public record Category([property: BsonId] Guid Id, string? Name, DateTime CreatedDate);

    public record Receipt([property: BsonId] Guid Id, string? EstablishmentName, DateTime ReceiptDate, IEnumerable<ReceiptItem>? ReceiptItems);

    public record ReceiptItem(Guid Id, Guid CategoryId, string ItemName, short Quantity, decimal ItemPrice, decimal TotalPrice, string Observation);
}
