using MediatR;
using SpendManagement.Application.Commands.Receipt.InputModels;
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
            var totalDiscounts = CalculateTotalDiscounts(request.Receipt);

            var receiptTotalPrice = request.Receipt.ReceiptItems.Sum(x => x.TotalPrice);

            request.Receipt.Total = receiptTotalPrice - totalDiscounts;

            var receiptCreateCommand = request.Receipt.ToCommand();

            await _receiptService.ValidateIfCategoryExistAsync(receiptCreateCommand.Receipt.CategoryId);

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            return receiptCreateCommand.Receipt.Id;
        }

        private static decimal CalculateTotalDiscounts(ReceiptInputModel receiptInputModel)
        {
            var makeDiscountBasedOnReceiptItems = receiptInputModel.ReceiptItems.Any(x => x.ItemDiscount != 0.0M);

            if (makeDiscountBasedOnReceiptItems)
            {
                var totalDiscounts = receiptInputModel.ReceiptItems.Sum(x => x.ItemDiscount);
                return totalDiscounts;
            }

            return receiptInputModel.Discount;
        }
    }
}
