using MediatR;

namespace SpendManagement.Application.Commands.Receipt.VariableReceipt.UseCases.DeleteReceipt
{
    public record DeleteVariableReceiptCommand(Guid Id) : IRequest;
}
