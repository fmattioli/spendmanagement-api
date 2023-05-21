using MediatR;
using SpendManagement.Application.InputModels;

namespace SpendManagement.Application.Commands.UpdateReceiptItem
{
    public record UpdateReceiptItemCommand(UpdateReceiptItemInputModel UpdateReceiptItemInputModel) : IRequest;
}
