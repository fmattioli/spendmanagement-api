using MediatR;
using SpendManagement.Application.InputModels;

namespace SpendManagement.Application.Commands.AddReceipt
{
    public record AddReceiptCommand(ReceiptInputModel AddSpentInputModel) : IRequest<Guid>;
}
