using MediatR;
using SpendManagement.Application.Commands.Receipt.InputModels;

namespace SpendManagement.Application.Commands.Receipt.UpdateReceipt
{
    public record UpdateReceiptCommand(Guid Id, UpdateReceiptInputModel UpdateReceiptInputModel) : IRequest;
}
