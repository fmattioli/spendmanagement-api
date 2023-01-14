using MediatR;

using Spents.Application.InputModels;

namespace Spents.Application.Commands.AddSpent
{
    public record AddSpentCommand(AddSpentInputModel addSpentInputModel) : IRequest<Guid>;

    
}
