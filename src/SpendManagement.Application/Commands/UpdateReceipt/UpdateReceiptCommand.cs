using MediatR;

namespace SpendManagement.Application.Commands.UpdateReceipt
{
    public record UpdateReceiptCommand(UpdateReceiptInputModel UpdateReceiptInputModel) : IRequest<Unit>;
}
