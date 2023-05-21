using MediatR;

namespace SpendManagement.Application.Commands.UpdateReceiptItem
{
    public record UpdateReceiptItemCommand(UpdateReceiptItemInputModel UpdateReceiptItemInputModel) : IRequest;
}
