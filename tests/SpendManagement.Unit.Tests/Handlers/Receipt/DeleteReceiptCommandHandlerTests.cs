using AutoFixture;
using FluentAssertions;
using Moq;
using SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt;
using SpendManagement.Application.Producers;

namespace SpendManagement.Unit.Tests.Handlers.Receipt
{
    public class DeleteReceiptCommandHandlerTests
    {
        private readonly DeleteReceiptCommandHandler handler;
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Fixture fixture = new();

        public DeleteReceiptCommandHandlerTests()
        {
            handler = new(commandProducerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProduceCategoryCommand()
        {
            // Arrange
            var receiptId = fixture.Create<Guid>();

            var request = fixture
                .Build<DeleteReceiptCommand>()
                .With(x => x.Id, receiptId)
                .Create();

            // Act
            await handler.Handle(request);

            // Assert
            commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<SpendManagement.Contracts.V1.Commands.ReceiptCommands.DeleteReceiptCommand>()),
                    Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_AnExeptionShoudOccur()
        {
            // Arrange
            var receiptId = fixture.Create<Guid>();

            var request = fixture
                .Build<DeleteReceiptCommand>()
                .With(x => x.Id, receiptId)
                .Create();

            commandProducerMock
                .Setup(x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.ReceiptCommands.DeleteReceiptCommand>()))
                .Throws<Exception>();

            // Act
            Func<Task> act = async () => await handler.Handle(request);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.ReceiptCommands.DeleteReceiptCommand>()),
                    Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
