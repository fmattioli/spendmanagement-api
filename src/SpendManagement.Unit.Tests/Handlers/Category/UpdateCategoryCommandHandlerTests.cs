using AutoFixture;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using Serilog;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel;
using Web.Contracts.Category;
using UpdateCategoryCommandHandler = SpendManagement.Application.Commands.Category.UseCases.UpdateCategory.UpdateCategoryCommand;
using UpdateCategoryCommand = SpendManagement.Contracts.V1.Commands.CategoryCommands.UpdateCategoryCommand;
using FluentValidation.Results;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;

namespace SpendManagement.Unit.Tests.Handlers.Category
{
    public class UpdateCategoryCommandHandlerTests
    {
        private readonly Application.Commands.Category.UseCases.UpdateCategory.UpdateCategoryCommandHandler handler;
        private readonly Fixture fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Mock<ISpendManagementReadModelClient> spendManagementReadModelClientMock = new();
        private readonly Mock<ILogger> loggerMock = new();
        private readonly Mock<ValidationResult> validationResult = new();

        public UpdateCategoryCommandHandlerTests() =>
            handler = new(commandProducerMock.Object, spendManagementReadModelClientMock.Object, loggerMock.Object, validationResult.Object);

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

            validationResult.Setup(x => x.IsValid).Returns(true);

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
        public async Task Handle_GivenAnInvalidJsonPatch_ShouldProduceAnException()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<CategoryResponse>();

            var categoryCommand = new UpdateCategoryCommandHandler(new UpdateCategoryInputModel
            {
                Id = Guid.NewGuid(),
                CategoryPatchDocument = jsonPatchDocument
            });

            var validationFailures = fixture.CreateMany<ValidationFailure>().ToList();
            var validationResult = new ValidationResult(validationFailures);

            var categoryResponse = fixture.Create<CategoryResponse>();

            spendManagementReadModelClientMock
                .Setup(x => x.GetCategoryAsync(It.IsAny<Guid>()))
                .ReturnsAsync(categoryResponse);

            this.validationResult.Setup(x => x.IsValid).Returns(false);

            //Act and assert
            await Assert.ThrowsAsync<JsonPatchInvalidException>(async () => await handler.Handle(categoryCommand, CancellationToken.None));
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

            validationResult.Setup(x => x.IsValid).Returns(true);

            //Act and assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(categoryCommand, CancellationToken.None));
        }
    }
}
