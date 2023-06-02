using MediatR;
using SpendManagement.Application.InputModels.Common;

namespace SpendManagement.Application.Commands.AddReceipt
{
    public record AddReceiptCommand(ReceiptInputModel AddSpentInputModel) : IRequest<Unit>;
}
