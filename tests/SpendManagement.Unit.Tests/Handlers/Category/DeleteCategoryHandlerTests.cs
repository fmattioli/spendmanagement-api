using AutoFixture;
using FluentAssertions;
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
            await handler.Handle(request, CancellationToken.None);

            // Assert
            commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.CategoryCommands.DeleteCategoryCommand>()),
                    Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_AnExeptionShoudOccur()
        {
            // Arrange
            var categoryId = fixture.Create<Guid>();
            var request = fixture
                .Build<DeleteCategoryCommand>()
                .With(x => x.Id, categoryId)
                .Create();

            commandProducerMock
                .Setup(x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.CategoryCommands.DeleteCategoryCommand>()))
                .Throws<Exception>();

            // Act
            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.CategoryCommands.DeleteCategoryCommand>()),
                    Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
