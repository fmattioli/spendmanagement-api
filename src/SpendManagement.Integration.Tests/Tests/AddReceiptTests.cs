using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;
using SpendManagement.Integration.Tests.Helpers;

namespace SpendManagement.Integration.Tests.Tests
{
    [Collection(nameof(SharedFixtureCollection))]
    public class AddReceiptTests : BaseTests<ReceiptInputModel>
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture;

        public AddReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture)
        {
            this.kafkaFixture = kafkaFixture;
            this.mongoDbFixture = mongoDbFixture;
        }

        [Fact(DisplayName = "On adding a valid receipt, a Kafka command should be produced.")]
        public async Task OnGivenAValidReceiptToBeCreated_ShouldBeProducedACreateReceiptCommand()
        {
            //Arrange
            var receipItems = fixture
                .Build<ReceiptItemInputModel>()
                .CreateMany();

            var receipt = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receipItems)
                .Create();

            var categories = receipItems.Select(x => new Category(x.CategoryId, fixture.Create<string>(), DateTime.UtcNow));

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

        [Fact(DisplayName = "On adding a invalid receipt with a category who does not exists, an error 404 should be produced.")]
        public async Task OnGivenAValidReceiptWithAnInvalidCategoryId_AnErrorShouldOccur()
        {
            //Arrange
            var receipItems = fixture
                .Build<ReceiptItemInputModel>()
                .CreateMany();

            var receipt = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receipItems)
                .Create();

            var categories = receipItems.Select(x => new Category(x.CategoryId, fixture.Create<string>(), DateTime.UtcNow));

            //Act
            var response = await PostAsync("/addReceipt", receipt);

            //Assert
            response.Should().HaveClientError();
        }
    }
}
