using MediatR;
using SpendManagement.Application.InputModels;

namespace SpendManagement.Application.Commands.UpdateReceipt
{
    public record UpdateReceiptCommand(UpdateReceiptInputModel UpdateReceiptInputModel) : IRequest;
}
