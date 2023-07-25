using MediatR;
using SpendManagement.Application.Commands.Receipt.InputModels;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public record AddReceiptCommand(ReceiptInputModel AddSpentInputModel) : IRequest<Guid>;
}
