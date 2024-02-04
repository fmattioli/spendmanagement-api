using MediatR;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddRecurringReceipt
{
    public class AddRecurringReceiptCommandHandler(ICommandProducer receiptProducer) : IRequestHandler<AddRecurringReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        public async Task<Guid> Handle(AddRecurringReceiptCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
