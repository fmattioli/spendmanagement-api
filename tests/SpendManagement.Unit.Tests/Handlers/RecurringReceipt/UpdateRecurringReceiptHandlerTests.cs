using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SpendManagement.Application.Commands.Receipt.RecurringReceipt.InputModel;
using SpendManagement.Application.Commands.Receipt.RecurringReceipt.UseCases.UpdateRecurringReceipt;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;
using SpendManagement.Client.SpendManagementReadModel;
using SpendManagement.WebContracts.Common;
using SpendManagement.WebContracts.Exceptions;
using SpendManagement.WebContracts.Receipt;

namespace SpendManagement.Unit.Tests.Handlers.RecurringReceipt
{
    public class UpdateRecurringReceiptHandlerTests
    {
        private readonly UpdateRecurringReceiptCommandHandler _handler;
        private readonly Mock<ISpendManagementReadModelClient> spendManagementReadModelClient = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Mock<IReceiptService> receiptService = new();
        private readonly Mock<IValidator<JsonPatchError>> _validatorMock = new();
        private readonly Fixture fixture = new();

        public UpdateRecurringReceiptHandlerTests()
        {
            _handler = new(commandProducerMock.Object, spendManagementReadModelClient.Object, receiptService.Object, _validatorMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldProduceReceiptUpdateCommand()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<RecurringReceiptResponse>();

            var receiptCommand = new UpdateRecurringReceiptCommand(Guid.NewGuid(), new UpdateRecurringReceiptInputModel
            {
                RecurringReceiptPatchDocument = jsonPatchDocument
            });

            var receiptResponse = fixture.Create<PagedResult<RecurringReceiptResponse>>();

            spendManagementReadModelClient
                .Setup(x => x.GetRecurringReceiptAsync(It.IsAny<Guid>()))
                .ReturnsAsync(receiptResponse);

            receiptService
                .Setup(x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            //Act
            await _handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            spendManagementReadModelClient
                .Verify(x => x.GetRecurringReceiptAsync(It.IsAny<Guid>()),
                Times.Once());

            commandProducerMock
               .Verify(
                   x => x.ProduceCommandAsync(It.IsAny<Contracts.V1.Commands.RecurringReceiptCommands.UpdateRecurringReceiptCommand>()),
                   Times.Once);

            receiptService
                .Verify(x => x.ValidateIfCategoryExistAsync(It.IsAny<Guid>()),
                Times.Exactly(1));

            spendManagementReadModelClient.VerifyNoOtherCalls();

            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_WhenReceiptIdWasNotFound_AnExceptionShouldOccur()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<RecurringReceiptResponse>();

            var receiptCommand = new UpdateRecurringReceiptCommand(Guid.NewGuid(), new UpdateRecurringReceiptInputModel
            {
                RecurringReceiptPatchDocument = jsonPatchDocument
            });

            spendManagementReadModelClient
               .Setup(x => x.GetRecurringReceiptAsync(It.IsAny<Guid>()))
               .Throws<NotFoundException>();

            //Act
            Func<Task> act = async () => await _handler.Handle(receiptCommand, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_WhenCategoryIdWasNotFound_AnExceptionShouldOccur()
        {
            //Arrange
            var jsonPatchDocument = new JsonPatchDocument<RecurringReceiptResponse>();

            var receiptCommand = new UpdateRecurringReceiptCommand(Guid.NewGuid(), new UpdateRecurringReceiptInputModel
            {
                RecurringReceiptPatchDocument = jsonPatchDocument
            });

            var receiptResponse = fixture.Create<PagedResult<RecurringReceiptResponse>>();

            spendManagementReadModelClient
                .Setup(x => x.GetRecurringReceiptAsync(It.IsAny<Guid>()))
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
