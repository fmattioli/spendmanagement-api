using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;

namespace SpendManagement.Application.Commands.RecurringReceipt.UseCases.AddRecurringReceipt
{
    public class AddRecurringReceiptCommandHandler(ICommandProducer receiptProducer, IReceiptService receiptService) : IRequestHandler<AddRecurringReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        private readonly IReceiptService _receiptService = receiptService;

        public async Task<Guid> Handle(AddRecurringReceiptCommand request, CancellationToken cancellationToken)
        {
            var addRecurringReceiptCommand = request.RecurringReceipt.ToCommand();
            await _receiptService.ValidateIfCategoryExistAsync(request.RecurringReceipt.CategoryId);
            await _receiptProducer.ProduceCommandAsync(addRecurringReceiptCommand);
            return addRecurringReceiptCommand.RecurringReceipt.Id;
        }
    }
}
