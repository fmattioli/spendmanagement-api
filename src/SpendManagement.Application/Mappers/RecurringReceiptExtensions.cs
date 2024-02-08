using SpendManagement.Application.Commands.RecurringReceipt.InputModel;
using SpendManagement.Contracts.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using MediatRCommands = SpendManagement.Application.Commands.RecurringReceipt.UseCases.DeleteRecurringReceipt;
namespace SpendManagement.Application.Mappers
{
    public static class RecurringReceiptExtensions
    {
        public static CreateRecurringReceiptCommand ToCommand(this RecurringReceiptInputModel reccuringReceiptInputModel)
        {
            var recurringReceiptCommand = new RecurringReceipt(reccuringReceiptInputModel.Id == Guid.Empty ? Guid.NewGuid() : reccuringReceiptInputModel.Id, 
                reccuringReceiptInputModel.CategoryId,
                reccuringReceiptInputModel.EstablishmentName, 
                reccuringReceiptInputModel.DateInitialRecurrence,
                reccuringReceiptInputModel.DateEndRecurrence,
                reccuringReceiptInputModel.RecurrenceTotalPrice,
                reccuringReceiptInputModel.Observation
                );

            return new CreateRecurringReceiptCommand(recurringReceiptCommand);
        }

        public static DeleteRecurringReceiptCommand ToCommand(this MediatRCommands.DeleteRecurringReceiptCommand deleteReceiptCommand)
        {
            return new DeleteRecurringReceiptCommand(deleteReceiptCommand.Id);
        }
    }
}
