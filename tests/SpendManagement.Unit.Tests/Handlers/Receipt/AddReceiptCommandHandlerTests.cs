using AutoFixture;
using FluentAssertions;

using Moq;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;
using CreateReceiptCommand = SpendManagement.Contracts.V1.Commands.ReceiptCommands.CreateReceiptCommand;
namespace SpendManagement.Unit.Tests.Handlers.Receipt
{
    public class AddReceiptCommandHandlerTests
    {
        private readonly AddReceiptCommandHandler handler;
        private readonly Fixture fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Mock<IReceiptService> receiptServiceMock = new();

        public AddReceiptCommandHandlerTests()
        {
            handler = new(commandProducerMock.Object, receiptServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProduceCreateReceiptCommand()
        {
            // Arrange
            var receiptInputModel = fixture
                .Build<ReceiptInputModel>()
                .Create();

            var receiptCommand = fixture
                .Build<AddReceiptCommand>()
                .With(x => x.Receipt, receiptInputModel)
                .Create();

            receiptServiceMock
                .Setup(x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            receiptServiceMock
                .Setup(x => x.CalculateReceiptTotals(It.IsAny<ReceiptInputModel>()))
                .Returns(receiptInputModel);

            //Act
            await handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            receiptServiceMock.Verify(
                x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()), Times.Exactly(1));
            
            receiptServiceMock.Verify(
                x => x.CalculateReceiptTotals(It.IsAny<ReceiptInputModel>()), Times.Exactly(1));

            commandProducerMock
               .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<CreateReceiptCommand>()),
                   Times.Once);
        }

        [Fact]
        public async Task HandleShouldHandleError()
        {
            // Arrange
            var receiptInputModel = fixture.Create<ReceiptInputModel>();
            var request = fixture
                .Build<AddReceiptCommand>()
                .Create();

            commandProducerMock
                .Setup(x => x.ProduceCommandAsync(It.IsAny<CreateReceiptCommand>()))
                .Throws<Exception>();

            receiptServiceMock
                .Setup(x => x.CalculateReceiptTotals(It.IsAny<ReceiptInputModel>()))
                .Returns(receiptInputModel);

            // Act
            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);
            await act.Should().ThrowAsync<Exception>();

            //Assert
            commandProducerMock.Verify(
                x => x.ProduceCommandAsync(It.IsAny<CreateReceiptCommand>()),
                Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
