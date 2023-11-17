using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Integration.Tests.Fixtures;
using SpendManagement.Integration.Tests.Helpers;

namespace SpendManagement.Integration.Tests.Handlers.Category
{
    [Collection(nameof(SharedFixtureCollection))]
    public class DeleteCategoryTests : BaseTests<CategoryInputModel>
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture;

        public DeleteCategoryTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture)
        {
            this.kafkaFixture = kafkaFixture;
            this.mongoDbFixture = mongoDbFixture;
        }

        [Fact(DisplayName = "On deleting a valid category Id, a Kafka command should be produced.")]
        private async Task OnGivenAValidCategoryId_ShouldBeProducedADeleteCategoryCommand()
        {
            //Arrange
            var categoryInputModel = fixture
                .Build<CategoryInputModel>()
                .Create();

            //Act
            var response = await DeleteAsync("/deleteCategory", categoryInputModel.Id);

            //Assert
            response
                .Should()
                .BeSuccessful();

            var categoryDeleteCommand = kafkaFixture.Consume<DeleteCategoryCommand>(
            (command, _) =>
               command.Id == categoryInputModel.Id &&
               command.RoutingKey == categoryInputModel.Id.ToString());

            categoryDeleteCommand
                .Should()
                .NotBeNull();
        }
    }
}
