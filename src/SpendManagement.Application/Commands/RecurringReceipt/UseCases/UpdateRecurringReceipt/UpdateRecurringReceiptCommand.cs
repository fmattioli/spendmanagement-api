using MediatR;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Commands.RecurringReceipt.InputModel;

namespace SpendManagement.Application.Commands.RecurringReceipt.UseCases.UpdateRecurringReceipt
{
    public record UpdateRecurringReceiptCommand(Guid Id, UpdateRecurringReceiptInputModel UpdateReceiptInputModel) : IRequest;
}
