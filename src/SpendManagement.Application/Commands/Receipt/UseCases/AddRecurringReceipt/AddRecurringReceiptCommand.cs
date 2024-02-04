using MediatR;
using SpendManagement.Application.Commands.Receipt.InputModels;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddRecurringReceipt
{
    public record AddRecurringReceiptCommand(RecurringReceiptInputModel Receipt) : IRequest<Guid>;
}
