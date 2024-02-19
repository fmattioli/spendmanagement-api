using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Handlers.Category
{
    [Collection(nameof(SharedFixtureCollection))]
    public class DeleteCategoryTest(KafkaFixture kafkaFixture, HttpFixture httpFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture = kafkaFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact(DisplayName = "On deleting a valid category Id, a Kafka command should be produced.")]
        private async Task OnGivenAValidCategoryId_ShouldBeProducedADeleteCategoryCommand()
        {
            //Arrange
            var categoryInputModel = fixture
                .Build<CategoryInputModel>()
                .Create();

            //Act
            var response = await _httpFixture.DeleteAsync("/deleteCategory", categoryInputModel.Id);

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
