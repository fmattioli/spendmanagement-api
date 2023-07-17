using MediatR;

namespace SpendManagement.Application.Commands.UpdateReceiptItem
{
    public class UpdateReceiptCommandItemHandler : IRequest
    {
        public async Task Handle(UpdateReceiptItemCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
