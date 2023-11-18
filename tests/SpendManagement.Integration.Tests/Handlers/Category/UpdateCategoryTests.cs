using AutoFixture;

using FluentAssertions;

using Newtonsoft.Json;

using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.Integration.Tests.Fixtures;
using SpendManagement.Integration.Tests.Helpers;

namespace SpendManagement.Integration.Tests.Handlers.Category
{
    [Collection(nameof(SharedFixtureCollection))]
    public class UpdateCategoryTests : BaseTests<CategoryInputModel>
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture;

        public UpdateCategoryTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture)
        {
            this.kafkaFixture = kafkaFixture;
            this.mongoDbFixture = mongoDbFixture;
        }

        [Fact(DisplayName = "On updating a valid category, a Kafka command should be produced.")]
        public async Task OnGivenAValidCategoryToBeUpdated_ShouldBeProducedAnUpdateCategoryCommand()
        {
            //Arrange
            var categoryId = fixture.Create<Guid>();

            var category = fixture
                .Build<Fixtures.Category>()
                .With(x => x.Id, categoryId)
                .With(x => x.Name, "Whatever name")
                .With(x => x.CreatedDate, DateTime.Now)
                .Create();

            await mongoDbFixture.InsertCategory(category);

            var categoryName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/Name", op = "replace", value = categoryName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await PatchAsync("/updateCategory", categoryId, jsonString);

            //Assert
            response.Should().BeSuccessful();

            var categoryUpdateCommand = kafkaFixture.Consume<UpdateCategoryCommand>(
            (command, _) =>
                command.Category.Id == categoryId &&
                command.Category.Name == categoryName &&
                command.RoutingKey == categoryId.ToString());

            categoryUpdateCommand.Should().NotBeNull();
        }

        [Fact(DisplayName = "On updating a invalid category, an error should occur")]
        public async Task OnGivenAInValidCategoryToBeUpdated_ShouldBeOccurAnError()
        {
            //Arrange
            var categoryId = fixture.Create<Guid>();

            var category = fixture
                .Build<Fixtures.Category>()
                .With(x => x.Id, categoryId)
                .With(x => x.Name, "Whatever name")
                .With(x => x.CreatedDate, DateTime.Now)
                .Create();

            var categoryName = fixture.Create<string>();

            var jsonItems = new List<object>
            {
                new { path = "/Name", op = "replace", value = categoryName }
            };

            string jsonString = JsonConvert.SerializeObject(jsonItems, Formatting.Indented);

            //Act
            var response = await PatchAsync("/updateCategory", categoryId, jsonString);

            //Assert
            response.Should().HaveClientError();
            response.Should().HaveClientError("GetCategory");
        }
    }
}
