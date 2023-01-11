using MediatR;
using Spents.Application.Commands.AddSpent;

namespace Spents.Application.Services
{
    public class AddSpentsCommandHandler : IRequestHandler<AddSpentCommand, Unit>
    {
        public Task<Unit> Handle(AddSpentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
