using MediatR;
using Spents.Application.InputModels;

namespace Spents.Application.Commands.AddReceipt
{
    public record AddReceiptCommand(ReceiptInputModel AddSpentInputModel) : IRequest<Guid>;

    
}
