using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Handlers.Receipts
{
    [Collection(nameof(SharedFixtureCollection))]
    public class AddReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture, HttpFixture httpFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture = kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture = mongoDbFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact(DisplayName = "On adding a valid receipt, a Kafka command should be produced.")]
        public async Task OnGivenAValidReceiptToBeCreated_ShouldBeProducedACreateReceiptCommand()
        {
            //Arrange
            var receipItems = fixture
                .Build<ReceiptItemInputModel>()
                .With(x => x.ItemPrice, 50)
                .With(x => x.ItemDiscount, 10)
                .CreateMany();

            var receipt = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receipItems)
                .With(x => x.Discount, 0.0M)
                .Create();

            var category = new Fixtures.Category(receipt.CategoryId, fixture.Create<string>(), DateTime.UtcNow);

            await mongoDbFixture.InsertCategory(category);

            //Act
            var response = await _httpFixture.PostAsync("/addReceipt", receipt);

            //Assert
            response.Should().BeSuccessful();

            var receiptCommand = kafkaFixture.Consume<CreateReceiptCommand>(
            (command, _) =>
                command.Receipt.Id == receipt.Id &&
                command.Receipt.EstablishmentName == receipt.EstablishmentName &&
                command.RoutingKey == receipt.Id.ToString());

            receiptCommand
                .Should()
                .NotBeNull();

            receiptCommand.ReceiptItems
                .Should()
                .HaveCount(receipItems.Count());

            receiptCommand.ReceiptItems
                .Should()
                .BeEquivalentTo(receipItems,
                options => options
                    .Excluding(x => x.TotalPrice)
                );
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

            mongoDbFixture.AddReceiptToCleanUp(receipt.Id);

            //Act
            var response = await _httpFixture.PostAsync("/addReceipt", receipt);

            //Assert
            response.Should().HaveClientError();
        }
    }
}
