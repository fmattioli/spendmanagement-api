using MediatR;

using Spents.Application.InputModels;

namespace Spents.Application.Commands.AddSpent
{
    public record AddSpentCommand(AddReceiptInputModel AddSpentInputModel) : IRequest<Guid>;

    
}
