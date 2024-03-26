using MediatR;
using SpendManagement.Application.Commands.Receipt.RecurringReceipt.InputModel;

namespace SpendManagement.Application.Commands.Receipt.RecurringReceipt.UseCases.UpdateRecurringReceipt
{
    public record UpdateRecurringReceiptCommand(Guid Id, UpdateRecurringReceiptInputModel UpdateRecurringReceiptInputModel) : IRequest;
}
