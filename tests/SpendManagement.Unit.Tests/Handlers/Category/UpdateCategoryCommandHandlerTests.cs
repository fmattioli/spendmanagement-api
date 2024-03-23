using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel;
using SpendManagement.WebContracts.Category;
using SpendManagement.WebContracts.Common;
using SpendManagement.WebContracts.Exceptions;

using UpdateCategoryCommand = SpendManagement.Contracts.V1.Commands.CategoryCommands.UpdateCategoryCommand;
using UpdateCategoryCommandHandler = SpendManagement.Application.Commands.Category.UseCases.UpdateCategory.UpdateCategoryCommand;

namespace SpendManagement.Unit.Tests.Handlers.Category
{
    public class UpdateCategoryCommandHandlerTests
    {
        private readonly Application.Commands.Category.UseCases.UpdateCategory.UpdateCategoryCommandHandler handler;
        private readonly Fixture fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Mock<ISpendManagementReadModelClient> spendManagementReadModelClientMock = new();
        private readonly Mock<IValidator<JsonPatchError>> _validatorMock = new();

        public UpdateCategoryCommandHandlerTests() =>
            handler = new(commandProducerMock.Object, spendManagementReadModelClientMock.Object, _validatorMock.Object);

        [Fact]
        public async Task Handle_ShouldProduceCategoryUpdateCommand()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<CategoryResponse>();

            var categoryCommand = new UpdateCategoryCommandHandler(new UpdateCategoryInputModel
            {
                Id = Guid.NewGuid(),
                CategoryPatchDocument = jsonPatchDocument
            });

            var categoryResponse = fixture.Create<PagedResult<CategoryResponse>>();

            spendManagementReadModelClientMock
                .Setup(x => x.GetCategoriesAsync(It.IsAny<Guid>()))
                .ReturnsAsync(categoryResponse);

            //Act
            await handler.Handle(categoryCommand, CancellationToken.None);

            //Assert
            spendManagementReadModelClientMock
                .Verify(x => x.GetCategoriesAsync(It.IsAny<Guid>()),
                Times.Once());

            commandProducerMock
               .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<UpdateCategoryCommand>()),
                   Times.Once);

            spendManagementReadModelClientMock.VerifyNoOtherCalls();
            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_WhenCategoryWasNotFound_ShouldProduceNotFoundException_CategoryUpdateCommand()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<CategoryResponse>();

            var categoryCommand = new UpdateCategoryCommandHandler(new UpdateCategoryInputModel
            {
                Id = Guid.NewGuid(),
                CategoryPatchDocument = jsonPatchDocument
            });

            var categoryResponse = fixture.Create<CategoryResponse>();

            spendManagementReadModelClientMock
                .Setup(x => x.GetCategoriesAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new NotFoundException());

            //Act
            Func<Task> act = async () => await handler.Handle(categoryCommand, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
