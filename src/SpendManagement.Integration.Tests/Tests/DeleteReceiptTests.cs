using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Integration.Tests.Fixtures;
using SpendManagement.Integration.Tests.Helpers;

namespace SpendManagement.Integration.Tests.Tests
{
    [Collection(nameof(SharedFixtureCollection))]
    public class DeleteReceiptTests : BaseTests<ReceiptInputModel>
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture;

        public DeleteReceiptTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture)
        {
            this.kafkaFixture = kafkaFixture;
            this.mongoDbFixture = mongoDbFixture;
        }

        [Fact(DisplayName = "On deleting a valid receipt with valid Guid provided, a Kafka command should be produced.")]
        public async Task OnGivenAValidGuidToBeDeleted_DeleteReceiptCommandShouldBeProduced()
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
    }
}
