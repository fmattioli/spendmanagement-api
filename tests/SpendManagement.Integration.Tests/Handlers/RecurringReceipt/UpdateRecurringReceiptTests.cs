using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Handlers.RecurringReceipt
{
    [Collection(nameof(SharedFixtureCollection))]
    public class UpdateRecurringReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture, HttpFixture httpFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture = kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture = mongoDbFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact]
        public async Task OnGivenAValidRecurringReceiptToBeUpdated_ShouldBeProducedAnUpdateRecurringReceiptCommand()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();
            var categoryName = fixture.Create<string>();

            var recurringReceipt = fixture
                .Build<Fixtures.RecurringReceipt>()
                .With(x => x.Id, receiptId)
                .With(x => x.EstablishmentName, "Whatever name")
                .Create();

            var category = new Fixtures.Category(recurringReceipt.CategoryId, categoryName, DateTime.UtcNow);

            await Task.WhenAll(
                mongoDbFixture.InsertCategory(category),
                mongoDbFixture.InsertRecurringReceipt(recurringReceipt));

            var newEstablishmentName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/EstablishmentName", op = "replace", value = newEstablishmentName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await _httpFixture.PatchAsync("/updateRecurringReceipt", receiptId, jsonString);

            //Assert
            response
                .Should()
                .BeSuccessful();

            var receiptCommand = kafkaFixture.Consume<UpdateRecurringReceiptCommand>(
            (command, _) =>
                command.RecurringReceipt.Id == recurringReceipt.Id &&
                command.RecurringReceipt.EstablishmentName == newEstablishmentName &&
                command.RoutingKey == recurringReceipt.Id.ToString());

            receiptCommand
                .Should()
                .NotBeNull();

            receiptCommand.RecurringReceipt.Id
               .Should()
               .Be(recurringReceipt.Id);

            receiptCommand.RecurringReceipt.EstablishmentName
                .Should()
                .Be(recurringReceipt.EstablishmentName);

            receiptCommand.RecurringReceipt.CategoryId
                .Should()
                .Be(recurringReceipt.CategoryId);

            receiptCommand.RecurringReceipt.DateInitialRecurrence
                .Should()
                .Be(recurringReceipt.DateInitialRecurrence);

            receiptCommand.RecurringReceipt.DateEndRecurrence
                .Should()
                .Be(recurringReceipt.DateEndRecurrence);

            receiptCommand.RecurringReceipt.DateEndRecurrence
                .Should()
                .Be(recurringReceipt.DateEndRecurrence);

            receiptCommand.RecurringReceipt.RecurrenceTotalPrice
                .Should()
                .Be(recurringReceipt.RecurrenceTotalPrice);

            receiptCommand.RecurringReceipt.Observation
                .Should()
                .Be(recurringReceipt.Observation);

            receiptCommand.RecurringReceipt
                .Should()
                .BeEquivalentTo(recurringReceipt);
        }

        [Fact]
        public async Task OnGivenAInvalidRecurringReceiptToBeUpdated_AnErrorShouldOccur()
        {
            //Arrange
            var receiptId = fixture.Create<Guid>();
            var categoryName = fixture.Create<string>();

            var receipt = fixture
                .Build<Fixtures.RecurringReceipt>()
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
            var response = await _httpFixture.PatchAsync("/updateRecurringReceipt", receiptId, jsonString);

            //Assert
            response
                .Should()
                .HaveClientError();

            response
                .Should()
                .HaveClientError("GetRecurringReceipt");
        }

        [Fact]
        public async Task OnGivenAInvalidRecurringReceiptToBeUpdated_WithAnInvalidCategory_AnErrorShouldOccur()
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
            var response = await _httpFixture.PatchAsync("/updateRecurringReceipt", receiptId, jsonString);

            //Assert
            response
                .Should()
                .HaveClientError();

            response
                .Should()
                .HaveClientError("GetCategory");
        }
    }
}
