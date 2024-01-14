using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public class AddReceiptCommandHandler(ICommandProducer receiptProducer, IReceiptService receiptService) : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        private readonly IReceiptService _receiptService = receiptService;

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.Receipt.ToCommand();

            var receiptTotalPrice = receiptCreateCommand.ReceiptItems.Sum(x => x.TotalPrice);

            var totalDiscounts = CalculateTotalDiscounts(receiptCreateCommand);

            receiptCreateCommand.Receipt.Total = receiptTotalPrice - totalDiscounts;

            await _receiptService.ValidateIfCategoryExistAsync(receiptCreateCommand.Receipt.CategoryId);

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            return receiptCreateCommand.Receipt.Id;
        }

        private static decimal CalculateTotalDiscounts(CreateReceiptCommand receiptCreateCommand)
        {
            var makeDiscountBasedOnReceiptItems = receiptCreateCommand.ReceiptItems.Any(x => x.ItemDiscount != 0.0M);

            if (makeDiscountBasedOnReceiptItems)
            {
                var totalDiscounts = receiptCreateCommand.ReceiptItems.Sum(x => x.ItemDiscount);
                return totalDiscounts;
            }

            return receiptCreateCommand.Receipt.Discount;
        }
    }
}
