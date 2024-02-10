using MediatR;
using SpendManagement.Application.Commands.RecurringReceipt.InputModel;

namespace SpendManagement.Application.Commands.RecurringReceipt.UseCases.AddRecurringReceipt
{
    public record AddRecurringReceiptCommand(RecurringReceiptInputModel RecurringReceipt) : IRequest<Guid>;
}
