using AutoFixture;
using FluentAssertions;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Handlers.RecurringReceipt
{
    [Collection(nameof(SharedFixtureCollection))]
    public class DeleteRecurringReceiptTests(KafkaFixture kafkaFixture, HttpFixture httpFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture = kafkaFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact(DisplayName = "On deleting a valid receipt with valid Guid provided, a Kafka command should be produced.")]
        public async Task OnGivenAValidGuidToBeDeleted_DeleteReceiptCommandShouldBeProduced()
        {
            //Arrange
            var receiptId = fixture
                .Create<Guid>();

            //Act
            var response = await _httpFixture.DeleteAsync("/deleteReceipt", receiptId);

            //Assert
            response.Should().BeSuccessful();

            var recurringReceiptCommand = kafkaFixture.Consume<DeleteRecurringReceiptCommand>(
            (command, _) =>
                command.RoutingKey == receiptId.ToString());

            recurringReceiptCommand
                .Should()
                .NotBeNull();
        }
    }
}
