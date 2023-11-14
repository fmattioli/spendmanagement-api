using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
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

        [Fact(DisplayName = "On creating a valid receipt with valid category, a Kafka command should be produced.")]
        public async Task OnGivenAValidReceipt_ShouldBeProducedACreateReceiptCommand()
        {
            //Arrange
            var receipItems = fixture
                .Build<ReceiptItemInputModel>()
                .CreateMany();

            var receipt = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receipItems)
                .Create();

            var categories = receipItems.Select(x => new Category
            {
                CreatedDate = DateTime.UtcNow,
                Id = x.CategoryId,
                Name = fixture.Create<string>()
            });

            await this.mongoDbFixture.InsertCategory(categories);

            //Act
            var response = await PostAsync("/addReceipt", receipt);

            //Assert
            response.Should().BeSuccessful();

            var receiptCommand = this.kafkaFixture.Consume<CreateReceiptCommand>(
            (command, _) =>
                command.Receipt.Id == receipt.Id &&
                command.Receipt.EstablishmentName == receipt.EstablishmentName &&
                command.RoutingKey == receipt.Id.ToString());

            receiptCommand.Should().NotBeNull();
            receiptCommand.ReceiptItems.Should().HaveCount(receipItems.Count());
            receiptCommand.Receipt.Should().BeEquivalentTo(receipt, options
                => options
                    .Excluding(x => x.ReceiptItems));
            receiptCommand.ReceiptItems.Should().BeEquivalentTo(receipItems);
        }

        [Fact(DisplayName = "On creating a receipt and a category does not exists, an error 404 should be produced.")]
        public async Task OnGivenAValidReceiptWithAnInvalidCategoryId_AndAnErrorShouldOccur()
        {
            //Arrange
            var receipItems = fixture
                .Build<ReceiptItemInputModel>()
                .CreateMany();

            var receipt = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receipItems)
                .Create();

            var categories = receipItems.Select(x => new Category
            {
                CreatedDate = DateTime.UtcNow,
                Id = x.CategoryId,
                Name = fixture.Create<string>()
            });

            //Act
            var response = await PostAsync("/addReceipt", receipt);

            //Assert
            response.Should().HaveClientError();
        }
    }
}