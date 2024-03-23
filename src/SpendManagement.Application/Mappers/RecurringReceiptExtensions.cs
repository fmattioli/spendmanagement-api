using SpendManagement.Application.Commands.RecurringReceipt.InputModel;
using SpendManagement.Contracts.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.WebContracts.Receipt;
using MediatRCommands = SpendManagement.Application.Commands.RecurringReceipt.UseCases.DeleteRecurringReceipt;

namespace SpendManagement.Application.Mappers
{
    public static class RecurringReceiptExtensions
    {
        public static CreateRecurringReceiptCommand ToCommand(this RecurringReceiptInputModel recurringReceiptInputModel)
        {
            var recurringReceiptCommand = new RecurringReceipt(recurringReceiptInputModel.Id == Guid.Empty ? Guid.NewGuid() : recurringReceiptInputModel.Id, 
                recurringReceiptInputModel.CategoryId,
                recurringReceiptInputModel.EstablishmentName, 
                recurringReceiptInputModel.DateInitialRecurrence,
                recurringReceiptInputModel.DateEndRecurrence,
                recurringReceiptInputModel.RecurrenceTotalPrice,
                recurringReceiptInputModel.Observation
                );

            return new CreateRecurringReceiptCommand(recurringReceiptCommand);
        }

        public static UpdateRecurringReceiptCommand ToCommand(this RecurringReceiptResponse receiptResponse)
        {
            var recurringReceipt = new RecurringReceipt(receiptResponse.Id,
                receiptResponse.CategoryId,
                receiptResponse.EstablishmentName,
                receiptResponse.DateInitialRecurrence,
                receiptResponse.DateEndRecurrence,
                receiptResponse.RecurrenceTotalPrice,
                receiptResponse.Observation);

            return new UpdateRecurringReceiptCommand(recurringReceipt);
        }

        public static DeleteRecurringReceiptCommand ToCommand(this MediatRCommands.DeleteRecurringReceiptCommand deleteReceiptCommand)
        {
            return new DeleteRecurringReceiptCommand(deleteReceiptCommand.Id);
        }
    }
}
