using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;
using SpendManagement.Integration.Tests.Helpers;

namespace SpendManagement.Integration.Tests.Handlers.Receipts
{
    [Collection(nameof(SharedFixtureCollection))]
    public class UpdateReceiptTests : BaseTests<ReceiptInputModel>
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture;

        public UpdateReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture)
        {
            this.kafkaFixture = kafkaFixture;
            this.mongoDbFixture = mongoDbFixture;
        }

        [Fact(DisplayName = "On updating a valid receipt, a Kafka command should be produced.")]
        public async Task OnGivenAValidReceiptToBeUpdated_ShouldBeProducedAnUpdateReceiptCommand()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();
            var categoryName = fixture.Create<string>();

            var receipt = fixture
                .Build<Receipt>()
                .With(x => x.Id, receiptId)
                .With(x => x.EstablishmentName, "Whatever name")
                .Create();

            var categories = receipt
                .ReceiptItems
                ?.Select(x =>
                    new Fixtures.Category(x.CategoryId, categoryName, DateTime.UtcNow));

            await Task.WhenAll(
                mongoDbFixture.InsertCategories(categories),
                mongoDbFixture.InsertReceipt(receipt));

            var newEstablishmentName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/EstablishmentName", op = "replace", value = newEstablishmentName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await PatchAsync("/updateReceipt", receiptId, jsonString);

            //Assert
            response.Should().BeSuccessful();

            var receiptCommand = kafkaFixture.Consume<UpdateReceiptCommand>(
            (command, _) =>
                command.Receipt.Id == receipt.Id &&
                command.Receipt.EstablishmentName == newEstablishmentName &&
                command.RoutingKey == receipt.Id.ToString());

            receiptCommand.Should().NotBeNull();
            receiptCommand.ReceiptItems.Should().HaveCount(receipt!.ReceiptItems!.Count());
        }

        [Fact(DisplayName = "On updating a invalid receipt that does not exists, an error should occur.")]
        public async Task OnGivenAInvalidReceiptToBeUpdated_AnErrorShouldOccur()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();
            var categoryName = fixture.Create<string>();

            var receipt = fixture
                .Build<Receipt>()
                .With(x => x.Id, receiptId)
                .With(x => x.EstablishmentName, "Whatever name")
                .Create();

            var categories = receipt
                .ReceiptItems
                ?.Select(x =>
                    new Fixtures.Category(x.CategoryId, categoryName, DateTime.UtcNow));

            await mongoDbFixture.InsertCategories(categories);

            var newEstablishmentName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/EstablishmentName", op = "replace", value = newEstablishmentName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await PatchAsync("/updateReceipt", receiptId, jsonString);

            //Assert
            response.Should().HaveClientError();
            response.Should().HaveClientError("GetReceipt");
        }

        [Fact(DisplayName = "On updating a invalid receipt that the categoryId does not exists, an error should occur.")]
        public async Task OnGivenAInvalidReceiptToBeUpdated_WithAnInvalidCategory_AnErrorShouldOccur()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();
            var categoryName = fixture.Create<string>();

            var receipt = fixture
                .Build<Receipt>()
                .With(x => x.Id, receiptId)
                .With(x => x.EstablishmentName, "Whatever name")
                .Create();

            var categories = receipt
                .ReceiptItems
                ?.Select(x =>
                    new Fixtures.Category(x.CategoryId, categoryName, DateTime.UtcNow));

            await mongoDbFixture.InsertReceipt(receipt);

            var newEstablishmentName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/EstablishmentName", op = "replace", value = newEstablishmentName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await PatchAsync("/updateReceipt", receiptId, jsonString);

            //Assert
            response.Should().HaveClientError();
            response.Should().HaveClientError("GetCategory");
        }
    }
}
