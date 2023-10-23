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

namespace SpendManagement.Unit.Tests.Handlers
{
    public class UpdateCategoryCommandHandlerTests
    {
        private readonly Application.Commands.Category.UseCases.UpdateCategory.UpdateCategoryCommandHandler handler;
        private readonly Fixture fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Mock<ISpendManagementReadModelClient> spendManagementReadModelClientMock = new();
        private readonly Mock<ILogger> loggerMock = new();

        public UpdateCategoryCommandHandlerTests() =>
            this.handler = new(this.commandProducerMock.Object, spendManagementReadModelClientMock.Object, loggerMock.Object);

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
            await this.handler.Handle(categoryCommand, CancellationToken.None);

            //Arrange
            this.spendManagementReadModelClientMock
                .Verify(x => x.GetCategoryAsync(It.IsAny<Guid>()),
                Times.Once());

            this.commandProducerMock
               .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<UpdateCategoryCommand>()),
                   Times.Once);

            this.spendManagementReadModelClientMock.VerifyNoOtherCalls();
            this.commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
