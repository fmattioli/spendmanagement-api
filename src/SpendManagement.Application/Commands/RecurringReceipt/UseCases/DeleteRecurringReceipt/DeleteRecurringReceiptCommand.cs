using MediatR;

namespace SpendManagement.Application.Commands.RecurringReceipt.UseCases.DeleteRecurringReceipt
{
    public record DeleteRecurringReceiptCommand(Guid Id) : IRequest;
}
