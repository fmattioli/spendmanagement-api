using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.RecurringReceipt.UseCases.AddRecurringReceipt
{
    public class AddRecurringReceiptCommandHandler(ICommandProducer receiptProducer) : IRequestHandler<AddRecurringReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        public async Task<Guid> Handle(AddRecurringReceiptCommand request, CancellationToken cancellationToken)
        {
            var addRecurringReceiptCommand = request.RecurringReceipt.ToCommand();
            await _receiptProducer.ProduceCommandAsync(addRecurringReceiptCommand);
            return addRecurringReceiptCommand.RecurringReceipt.Id;
        }
    }
}
