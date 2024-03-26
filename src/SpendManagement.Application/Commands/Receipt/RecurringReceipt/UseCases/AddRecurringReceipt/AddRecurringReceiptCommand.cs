using MediatR;
using SpendManagement.Application.Commands.Receipt.RecurringReceipt.InputModel;

namespace SpendManagement.Application.Commands.Receipt.RecurringReceipt.UseCases.AddRecurringReceipt
{
    public record AddRecurringReceiptCommand(RecurringReceiptInputModel RecurringReceipt) : IRequest<Guid>;
}
