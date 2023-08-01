using MediatR;

namespace SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt
{
    public record DeleteReceiptCommand(Guid Id) : IRequest<Unit>;
}
