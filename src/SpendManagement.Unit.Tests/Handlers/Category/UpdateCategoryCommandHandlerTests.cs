using AutoFixture;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel;
using Web.Contracts.Category;
using UpdateCategoryCommandHandler = SpendManagement.Application.Commands.Category.UseCases.UpdateCategory.UpdateCategoryCommand;
using UpdateCategoryCommand = SpendManagement.Contracts.V1.Commands.CategoryCommands.UpdateCategoryCommand;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using FluentValidation;

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

            var categoryResponse = fixture.Create<CategoryResponse>();

            spendManagementReadModelClientMock
                .Setup(x => x.GetCategoryAsync(It.IsAny<Guid>()))
                .ReturnsAsync(categoryResponse);

            //Act
            await handler.Handle(categoryCommand, CancellationToken.None);

            //Assert
            spendManagementReadModelClientMock
                .Verify(x => x.GetCategoryAsync(It.IsAny<Guid>()),
                Times.Once());

            commandProducerMock
               .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<UpdateCategoryCommand>()),
                   Times.Once);

            spendManagementReadModelClientMock.VerifyNoOtherCalls();
            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_ShouldProduceExceoption_CategoryUpdateCommand()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<CategoryResponse>();

            var categoryCommand = new UpdateCategoryCommandHandler(new UpdateCategoryInputModel
            {
                Id = Guid.NewGuid(),
                CategoryPatchDocument = jsonPatchDocument
            });

            var categoryResponse = fixture.Create<CategoryResponse>();

            _ = spendManagementReadModelClientMock
                .Setup(x => x.GetCategoryAsync(It.IsAny<Guid>()))
                .ReturnsAsync(null as CategoryResponse);

            //Act and assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(categoryCommand, CancellationToken.None));
        }
    }
}
