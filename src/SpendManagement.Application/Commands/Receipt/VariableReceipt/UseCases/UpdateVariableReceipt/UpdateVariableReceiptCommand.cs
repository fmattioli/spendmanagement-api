using MediatR;
using SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels;

namespace SpendManagement.Application.Commands.Receipt.VariableReceipt.UseCases.UpdateReceipt
{
    public record UpdateVariableReceiptCommand(Guid Id, UpdateVariableReceiptInputModel UpdateReceiptInputModel) : IRequest;
}
