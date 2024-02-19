using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.RecurringReceipt.InputModel;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Handlers.RecurringReceipt
{
    [Collection(nameof(SharedFixtureCollection))]
    public class AddRecurringReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture, HttpFixture httpFixture)
    {
        private readonly Fixture _fixture = new();
        private readonly KafkaFixture _kafkaFixture = kafkaFixture;
        private readonly MongoDbFixture _mongoDbFixture = mongoDbFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact]
        public async Task OnGivenAValidRecurringReceiptToBeCreated_ShouldBeProducedACreateReceiptCommand()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .Create();

            var category = new Fixtures.Category(recurringReceipt.CategoryId, _fixture.Create<string>(), DateTime.UtcNow);

            await _mongoDbFixture.InsertCategory(category);

            //Act
            var response = await _httpFixture.PostAsync("/addRecurringReceipt", recurringReceipt);

            //Assert
            response.Should().BeSuccessful();

            var receiptCommand = _kafkaFixture.Consume<CreateRecurringReceiptCommand>(
            (command, _) =>
                command.RecurringReceipt.Id == recurringReceipt.Id &&
                command.RecurringReceipt.EstablishmentName == recurringReceipt.EstablishmentName &&
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
        public async Task OnGivenAValidRecurringReceiptWithAnInexistingCategoryId_AnErrorShouldOccur()
        {
            //Arrange
            var receipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.DateInitialRecurrence, DateTime.Now)
                .With(x => x.DateEndRecurrence, DateTime.Now.AddMonths(3))
                .Create();

            //Act
            var response = await _httpFixture.PostAsync("/addReceipt", receipt);

            //Assert
            response
                .Should()
                .HaveClientError();
        }

        [Fact]
        public async Task OnGivenAnInvalidRecurringReceipt_AnErrorShouldOccur()
        {
            //Arrange
            var receipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.DateInitialRecurrence, DateTime.Now)
                .With(x => x.DateEndRecurrence, DateTime.Now.AddMonths(-2))
                .With(x => x.CategoryId, Guid.Empty)
                .With(x => x.EstablishmentName, string.Empty)
                .Create();

            //Act
            var response = await _httpFixture.PostAsync("/addReceipt", receipt);

            //Assert
            response
                .Should()
                .HaveClientError();
        }
    }
}
