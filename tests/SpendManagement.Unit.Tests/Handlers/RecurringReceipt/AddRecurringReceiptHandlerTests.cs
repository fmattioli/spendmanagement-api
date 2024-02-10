using AutoFixture;
using FluentAssertions;
using Moq;
using SpendManagement.Application.Commands.RecurringReceipt.InputModel;
using SpendManagement.Application.Commands.RecurringReceipt.UseCases.AddRecurringReceipt;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;

namespace SpendManagement.Unit.Tests.Handlers.RecurringReceipt
{
    public class AddRecurringReceiptHandlerTests
    {
        private readonly AddRecurringReceiptCommandHandler handler;
        private readonly Fixture _fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Mock<IReceiptService> receiptServiceMock = new();

        public AddRecurringReceiptHandlerTests()
        {
            handler = new(commandProducerMock.Object, receiptServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProduceCreateReceiptCommand()
        {
            // Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .Create();

            var receiptCommand = _fixture
                .Build<AddRecurringReceiptCommand>()
                .With(x => x.RecurringReceipt, recurringReceipt)
                .Create();

            receiptServiceMock
                .Setup(x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            //Act
            await handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            receiptServiceMock.Verify(
                x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()), Times.Exactly(1));
            commandProducerMock
            .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<CreateRecurringReceiptCommand>()),
                   Times.Once);
        }

        [Fact]
        public async Task HandleShouldHandleError()
        {
            // Arrange
            var receiptInputModel = _fixture.Create<RecurringReceiptInputModel>();
            var request = _fixture
                .Build<AddRecurringReceiptCommand>()
                .Create();

            commandProducerMock
                .Setup(x => x.ProduceCommandAsync(It.IsAny<CreateRecurringReceiptCommand>()))
                .Throws<Exception>();

            // Act
            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>();

            //Assert
            commandProducerMock.Verify(
                x => x.ProduceCommandAsync(It.IsAny<CreateRecurringReceiptCommand>()),
                Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
