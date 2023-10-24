using AutoFixture;
using Moq;
using SpendManagement.Application.Commands.Category.UseCases.DeleteCategory;
using SpendManagement.Application.Producers;

namespace SpendManagement.Unit.Tests.Handlers.Category
{
    public class DeleteCategoryHandlerTests
    {
        private readonly DeleteCategoryCommandHandler handler;
        private readonly Fixture fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();

        public DeleteCategoryHandlerTests()
        {
            handler = new(commandProducerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProduceCategoryDeleteCommand()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();
            var request = fixture
                .Build<DeleteCategoryCommand>()
                .With(x => x.Id, categoryId)
                .Create();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.CategoryCommands.DeleteCategoryCommand>()),
                    Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
