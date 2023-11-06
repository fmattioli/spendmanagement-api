using AutoFixture;
using FluentAssertions;

using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SpendManagement.Application.Commands.Category.InputModels;

using SpendManagement.Application.Commands.Category.UseCases.UpdateCategory;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.Receipt.Services;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel;

using Web.Contracts.Category;
using Web.Contracts.Receipt;

namespace SpendManagement.Unit.Tests.Handlers.Receipt
{
    public class UpdateReceiptCommandHandlerTests
    {
        private readonly UpdateReceiptCommandHandler _handler;
        private readonly Fixture fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Mock<IReceiptService> receiptService = new();
        private readonly Mock<ISpendManagementReadModelClient> spendManagementReadModelClientMock = new();
        private readonly Mock<IValidator<JsonPatchError>> _validatorMock = new();

        public UpdateReceiptCommandHandlerTests()
        {
            _handler = new(commandProducerMock.Object, spendManagementReadModelClientMock.Object, receiptService.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProduceReceiptUpdateCommand()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<ReceiptResponse>();

            var receiptCommand = new UpdateReceiptCommand(new UpdateReceiptInputModel
            {
                Id = Guid.NewGuid(),
                ReceiptPatchDocument = jsonPatchDocument
            });

            var receiptResponse = fixture.Create<ReceiptResponse>();

            spendManagementReadModelClientMock
                .Setup(x => x.GetReceiptAsync(It.IsAny<Guid>()))
                .ReturnsAsync(receiptResponse);

            receiptService
                .Setup(x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            //Act
            await _handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            spendManagementReadModelClientMock
                .Verify(x => x.GetReceiptAsync(It.IsAny<Guid>()),
                Times.Once());

            commandProducerMock
               .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.ReceiptCommands.UpdateReceiptCommand>()),
                   Times.Once);

            receiptService
                .Verify(x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()),
                Times.Exactly(receiptResponse.ReceiptItems.Count()));

            spendManagementReadModelClientMock.VerifyNoOtherCalls();

            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_WhenReceiptIdWasNotFound_AnExceptionShouldOccur()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<ReceiptResponse>();

            var receiptCommand = new UpdateReceiptCommand(new UpdateReceiptInputModel
            {
                Id = Guid.NewGuid(),
                ReceiptPatchDocument = jsonPatchDocument
            });

            spendManagementReadModelClientMock
               .Setup(x => x.GetReceiptAsync(It.IsAny<Guid>()))
               .ReturnsAsync(null as ReceiptResponse);

            //Act
            Func<Task> act = async () => await _handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_WhenCategoryIdIsInvalid_AnExceptionShouldOccur()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<ReceiptResponse>();

            var receiptCommand = new UpdateReceiptCommand(new UpdateReceiptInputModel
            {
                Id = Guid.NewGuid(),
                ReceiptPatchDocument = jsonPatchDocument
            });

            var receiptResponse = fixture.Create<ReceiptResponse>();

            spendManagementReadModelClientMock
                .Setup(x => x.GetReceiptAsync(It.IsAny<Guid>()))
                .ReturnsAsync(receiptResponse);

            receiptService
               .Setup(x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()))
               .Throws<HttpRequestException>();

            //Act
            Func<Task> act = async () => await _handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<HttpRequestException>();
        }
    }
}
