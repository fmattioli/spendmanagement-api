﻿using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Integration.Tests.Fixtures;

namespace SpendManagement.Integration.Tests.Handlers.Category
{
    [Collection(nameof(SharedFixtureCollection))]
    public class AddCategoryTests(KafkaFixture kafkaFixture, MongoDbFixture mongoDbFixture, HttpFixture httpFixture)
    {
        private readonly Fixture fixture = new();
        private readonly KafkaFixture kafkaFixture = kafkaFixture;
        private readonly MongoDbFixture mongoDbFixture = mongoDbFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact(DisplayName = "On adding a valid category, a Kafka command should be produced.")]
        private async Task OnGivenAValidCategory_ShouldBeProducedACreateCategoryCommand()
        {
            //Arrange
            var categoryInputModel = fixture
                .Build<CategoryInputModel>()
                .Create();

            mongoDbFixture.AddCategoryToCleanUp(categoryInputModel.Id);

            //Act
            var response = await _httpFixture.PostAsync("/addCategory", categoryInputModel);

            //Assert
            response
                .Should()
                .BeSuccessful();

            var categoryCommand = kafkaFixture.Consume<CreateCategoryCommand>(
            (command, _) =>
               command.Category.Id == categoryInputModel.Id &&
               command.RoutingKey == categoryInputModel.Id.ToString() &&
               command.Category.Name == categoryInputModel.Name);

            categoryCommand
                .Should()
                .NotBeNull();
        }
    }
}
