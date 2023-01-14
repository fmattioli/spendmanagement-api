using MediatR;
using Spents.Application.Commands.AddSpent;
using Spents.Core.Interfaces;

namespace Spents.Application.Services
{
    public class AddSpentsCommandHandler : IRequestHandler<AddSpentCommand, Guid>
    {
        private readonly IReceiptRepository spentRepository;

        public AddSpentsCommandHandler(IReceiptRepository spentRepository)
        {
            this.spentRepository = spentRepository;
        }
        public async Task<Guid> Handle(AddSpentCommand request, CancellationToken cancellationToken)
        {
            var spent = request.addSpentInputModel.ToEntity();
            return await spentRepository.AddReceipt(spent);
        }
    }
}
