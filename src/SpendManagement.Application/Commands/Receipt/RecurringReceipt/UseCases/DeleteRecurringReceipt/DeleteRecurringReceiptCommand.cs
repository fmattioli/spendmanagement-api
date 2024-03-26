using MediatR;

namespace SpendManagement.Application.Commands.Receipt.RecurringReceipt.UseCases.DeleteRecurringReceipt
{
    public record DeleteRecurringReceiptCommand(Guid Id) : IRequest;
}
