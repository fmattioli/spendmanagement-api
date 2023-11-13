using AutoFixture;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Tests
{
    [Collection(nameof(SharedFixtureCollection))]
    public class ReceiptIntegrationTests : BaseTests<ReceiptInputModel>
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture;

        public ReceiptIntegrationTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture)
        {
            this.kafkaFixture = kafkaFixture;
            this.mongoDbFixture = mongoDbFixture;
        }

        [Fact]
        public async Task OnAddAReceipt_ShouldReturnAValidGuid()
        {
            //Arrange
            var categoryId = fixture.Create<Guid>();

            var receipItems = fixture
                .Build<ReceiptItemInputModel>()
                .With(x => x.CategoryId, categoryId)
                .CreateMany(1);

            var receipt = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receipItems)
                .Create();

            var category = fixture
                .Build<Category>()
                .With(x => x.Id, categoryId)
                .Create();

            await this.mongoDbFixture.InsertCategory(category);

            //Act
            var response = await PostAsync("/addReceipt", receipt);

            ////Assert
            //var groupEvent = this.kafkaFixture.Consume<ICommand>(
            //    (d, _) => d.RoutingKey == receipt.Id.ToString());
        }
    }
}