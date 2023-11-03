using MediatR;
using SpendManagement.Application.Commands.Receipt.Services;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer;
        private readonly IReceiptService _receiptService;

        public AddReceiptCommandHandler(ICommandProducer receiptProducer, IReceiptService receiptService)
        {
            _receiptProducer = receiptProducer;
            _receiptService = receiptService;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.Receipt.ToCommand();

            await Task.WhenAll(
                request.Receipt.ReceiptItems.Select(x => _receiptService.ValidateIfCategoryExistAsync(x.CategoryId))
                );

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            return receiptCreateCommand.Receipt.Id;
        }
    }
}
