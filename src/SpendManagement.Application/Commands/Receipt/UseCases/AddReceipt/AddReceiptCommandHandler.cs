using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public class AddReceiptCommandHandler(ICommandProducer receiptProducer, IReceiptService receiptService) : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        private readonly IReceiptService _receiptService = receiptService;

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.Receipt.ToCommand();

            await _receiptService.ValidateIfCategoryExistAsync(receiptCreateCommand.Receipt.CategoryId);

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            return receiptCreateCommand.Receipt.Id;
        }
    }
}
