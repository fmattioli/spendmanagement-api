using AutoFixture;
using FluentAssertions;

using Newtonsoft.Json;

using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;
using SpendManagement.Integration.Tests.Helpers;

using System.Linq;

using Web.Contracts.Receipt;

namespace SpendManagement.Integration.Tests.Tests
{
    [Collection(nameof(SharedFixtureCollection))]
    public class ReceiptTests : BaseTests<ReceiptInputModel>
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture;

        public ReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture)
        {
            this.kafkaFixture = kafkaFixture;
            this.mongoDbFixture = mongoDbFixture;
        }

        [Fact(DisplayName = "On creating a valid receipt with valid category, a Kafka command should be produced.")]
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

        [Fact(DisplayName = "On creating a receipt and a category does not exists, an error 404 should be produced.")]
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

        [Fact(DisplayName = "On deleting a valid receipt with valid Guid provided, a Kafka command should be produced.")]
        public async Task OnGivenAReceiptGuidValid_ShouldBeProducedADeleteReceiptCommand()
        {
            //Arrange
            var receiptId = fixture
                .Create<Guid>();

            //Act
            var response = await DeleteAsync("/deleteReceipt", receiptId);

            //Assert
            response.Should().BeSuccessful();

            var receiptCommand = this.kafkaFixture.Consume<DeleteReceiptCommand>(
            (command, _) =>
                command.RoutingKey == receiptId.ToString());

            receiptCommand.Should().NotBeNull();
        }

        [Fact(DisplayName = "On updating a valid receipt with valid ReceiptId, a Kafka command should be produced.")]
        public async Task OnGivenAValidReceiptToBeUpdated_ShouldBeProducedACreateReceiptCommand()
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
                    new Category(x.CategoryId, categoryName, DateTime.UtcNow));

            await Task.WhenAll(
                this.mongoDbFixture.InsertCategory(categories),
                this.mongoDbFixture.InsertReceipt(receipt));

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

            var receiptCommand = this.kafkaFixture.Consume<UpdateReceiptCommand>(
            (command, _) =>
                command.Receipt.Id == receipt.Id &&
                command.Receipt.EstablishmentName == newEstablishmentName &&
                command.RoutingKey == receipt.Id.ToString());

            receiptCommand.Should().NotBeNull();
            receiptCommand.ReceiptItems.Should().HaveCount(receipt!.ReceiptItems!.Count());
        }
    }
}