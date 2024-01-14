using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Mappers;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.WebContracts.Category;

using CategoryCommand = SpendManagement.Application.Commands.Category.UseCases.DeleteCategory;

namespace SpendManagement.Unit.Tests.Mappers
{
    public class CategoryMapper
    {
        private readonly Fixture fixture = new();

        [Fact]
        public void ToCreateCategoryCommand_WithValidInputModel_ReturnsValidCommand()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();

            var categoryInputModel = fixture
                .Build<CategoryInputModel>()
                .With(x => x.Id, categoryId)
                .Create();

            var category = new Category(categoryInputModel.Id, categoryInputModel.Name);

            var expectedCommand = new CreateCategoryCommand(category);

            // Act
            var result = categoryInputModel.ToCreateCategoryCommand();

            // Assert
            result.CommandCreatedDate
                .ToShortDateString()
                .Should()
                .Be(expectedCommand.CommandCreatedDate.ToShortDateString());

            result.RoutingKey
                .Should()
                .Be(expectedCommand.RoutingKey);

            result.Category.Name
                .Should()
                .Be(expectedCommand.Category.Name);

            result.Category.Id
                .Should()
                .Be(expectedCommand.Category.Id);

            result.Category.CreatedDate.ToShortDateString()
                .Should()
                .Be(expectedCommand.Category.CreatedDate.ToShortDateString());
        }

        [Fact]
        public void ToDeleteCategoryCommand_WithValidInputModel_ReturnsValidCommand()
        {
            //Arrange
            var categoryId = fixture.Create<Guid>();
            var expectedCommand = new CategoryCommand.DeleteCategoryCommand(categoryId);

            //Act
            var result = expectedCommand.ToCommand();
            result.Should().BeEquivalentTo(expectedCommand);
        }

        [Fact]
        public void ToUpdateCategoryCommand_WithValidInputModel_ReturnsValidCommand()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();

            var categoryResponse = fixture
                .Build<CategoryResponse>()
                .With(x => x.Id, categoryId)
                .Create();

            var category = new Category(categoryResponse.Id, categoryResponse.Name ?? "NAME");
            var expectedCommand = new UpdateCategoryCommand(category);

            // Act
            var result = categoryResponse.ToUpdateCategoryCommand();

            // Assert
            result.CommandCreatedDate.ToShortDateString()
                .Should()
                .Be(expectedCommand.CommandCreatedDate.ToShortDateString());

            result.RoutingKey
                .Should()
                .Be(expectedCommand.RoutingKey);

            result.Category.Name
                .Should()
                .Be(expectedCommand.Category.Name);

            result.Category.CreatedDate.ToShortDateString()
                .Should()
                .Be(expectedCommand.Category.CreatedDate.ToShortDateString());
        }
    }
}
