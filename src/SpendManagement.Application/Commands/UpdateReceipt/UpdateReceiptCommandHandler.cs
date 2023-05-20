using MediatR;

namespace SpendManagement.Application.Commands.UpdateReceipt
{
    public class UpdateReceiptCommandHandler : IRequest
    {
        public async Task Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
