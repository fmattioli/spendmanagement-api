using MediatR;

namespace SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt
{
    public class DeleteReceiptCommandHandler : IRequestHandler<DeleteReceiptCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
