using AutoFixture;
using Moq;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Receipt.Services;
using SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt;
using SpendManagement.Application.Producers;
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
        public async Task Handle_ShouldProduceReceiptCommand()
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

            //Act
            await handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            receiptServiceMock.Verify(
                x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()), Times.Exactly(receiptInputModel.ReceiptItems.Count()));

            commandProducerMock
               .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<CreateReceiptCommand>()),
                   Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldHandleErrorCase()
        {
            // Arrange
            var receiptInputModel = fixture.Create<ReceiptInputModel>();
            var request = fixture.Build<AddReceiptCommand>()
                                 .Create();

            commandProducerMock
                .Setup(x => x.ProduceCommandAsync(It.IsAny<CreateReceiptCommand>()))
                .Throws<Exception>();

            // Act and assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            commandProducerMock.Verify(
                x => x.ProduceCommandAsync(It.IsAny<CreateReceiptCommand>()),
                Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
