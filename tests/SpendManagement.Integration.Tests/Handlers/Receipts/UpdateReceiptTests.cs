using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Handlers.Receipts
{
    [Collection(nameof(SharedFixtureCollection))]
    public class UpdateReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture, HttpFixture httpFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture = kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture = mongoDbFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

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

            var category = new Fixtures.Category(receipt.CategoryId, categoryName, DateTime.UtcNow);

            await Task.WhenAll(
                mongoDbFixture.InsertCategory(category),
                mongoDbFixture.InsertReceipt(receipt));

            var newEstablishmentName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/EstablishmentName", op = "replace", value = newEstablishmentName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await _httpFixture.PatchAsync("/updateReceipt", receiptId, jsonString);

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

            var category = new Fixtures.Category(receipt.CategoryId, categoryName, DateTime.UtcNow);

            await mongoDbFixture.InsertCategory(category);

            var newEstablishmentName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/EstablishmentName", op = "replace", value = newEstablishmentName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await _httpFixture.PatchAsync("/updateReceipt", receiptId, jsonString);

            //Assert
            response.Should().HaveClientError();
            response.Should().HaveClientError("GetReceipt");
        }

        [Fact(DisplayName = "On updating a invalid receipt that the categoryId does not exists, an error should occur.")]
        public async Task OnGivenAInvalidReceiptToBeUpdated_WithAnInvalidCategory_AnErrorShouldOccur()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();

            var receipt = fixture
                .Build<Receipt>()
                .With(x => x.Id, receiptId)
                .With(x => x.EstablishmentName, "Whatever name")
                .Create();

            await mongoDbFixture.InsertReceipt(receipt);

            var newEstablishmentName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/EstablishmentName", op = "replace", value = newEstablishmentName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await _httpFixture.PatchAsync("/updateReceipt", receiptId, jsonString);

            //Assert
            response.Should().HaveClientError();
            response.Should().HaveClientError("GetCategory");
        }
    }
}
