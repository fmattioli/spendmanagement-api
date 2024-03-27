using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;

namespace SpendManagement.Application.Commands.Receipt.VariableReceipt.UseCases.AddReceipt
{
    public class AddVariableReceiptCommandHandler(ICommandProducer receiptProducer, IReceiptService receiptService) : IRequestHandler<AddVariableReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        private readonly IReceiptService _receiptService = receiptService;

        public async Task<Guid> Handle(AddVariableReceiptCommand request, CancellationToken cancellationToken)
        {
            request.Receipt = _receiptService.CalculateReceiptTotals(request.Receipt);

            var receiptCreateCommand = request.Receipt.ToCommand();

            await _receiptService.ValidateIfCategoryExistAsync(receiptCreateCommand.Receipt.CategoryId);

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            return receiptCreateCommand.Receipt.Id;
        }
    }
}
