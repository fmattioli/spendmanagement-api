using AutoFixture;
using FluentAssertions;
using Moq;
using SpendManagement.Application.Commands.Receipt.RecurringReceipt.UseCases.DeleteRecurringReceipt;
using SpendManagement.Application.Producers;

namespace SpendManagement.Unit.Tests.Handlers.RecurringReceipt
{
    public class DeleteRecurringReceiptHandlerTests
    {

        private readonly DeleteRecurringReceiptCommandHandler _handler;
        private readonly Mock<ICommandProducer> _commandProducerMock = new();
        private readonly Fixture _fixture = new();
        public DeleteRecurringReceiptHandlerTests()
        {
            _handler = new(_commandProducerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProduceCategoryCommand()
        {
            // Arrange
            var receiptId = _fixture.Create<Guid>();

            var request = _fixture
                .Build<DeleteRecurringReceiptCommand>()
                .With(x => x.Id, receiptId)
                .Create();

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands.DeleteRecurringReceiptCommand>()),
                    Times.Once);

            _commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_AnExeptionShoudOccur()
        {
            // Arrange
            var receiptId = _fixture.Create<Guid>();

            var request = _fixture
                .Build<DeleteRecurringReceiptCommand>()
                .With(x => x.Id, receiptId)
                .Create();

            _commandProducerMock
                .Setup(x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.RecurringReceiptCommands.DeleteRecurringReceiptCommand>()))
                .Throws<Exception>();

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            _commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.RecurringReceiptCommands.DeleteRecurringReceiptCommand>()),
                    Times.Once);

            _commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
