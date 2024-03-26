using SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels;
using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Services
{
    public class ReceiptService(ISpendManagementReadModelClient spendManagementReadModelClient) : IReceiptService
    {
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient = spendManagementReadModelClient;

        public ReceiptInputModel CalculateReceiptTotals(ReceiptInputModel receiptInputModel)
        {
            VerifyIfItIsNecessaryApplyDiscountsBasedOnReceiptItem(receiptInputModel);
            ProvideDiscountOnReceiptItemTotalPrice(receiptInputModel);
            CalculateReceiptTotalPrice(receiptInputModel);
            return receiptInputModel;
        }
        public async Task ValidateIfCategoryExistAsync(Guid categoryId)
        {
            await _spendManagementReadModelClient
                .GetCategoriesAsync(categoryId);
        }

        private static void VerifyIfItIsNecessaryApplyDiscountsBasedOnReceiptItem(ReceiptInputModel receiptInputModel)
        {
            var makeDiscountBasedOnReceiptItems = receiptInputModel.ReceiptItems.Any(x => x.ItemDiscount != 0.0M);

            if (makeDiscountBasedOnReceiptItems)
            {
                var totalDiscounts = receiptInputModel.ReceiptItems.Sum(x => x.ItemDiscount);
                receiptInputModel.Discount = totalDiscounts;
            }
        }

        private static void ProvideDiscountOnReceiptItemTotalPrice(ReceiptInputModel receiptInputModel)
        {
            foreach (var item in receiptInputModel.ReceiptItems)
            {
                item.TotalPrice = (item.Quantity * item.ItemPrice) - item.ItemDiscount;
            }
        }

        private static void CalculateReceiptTotalPrice(ReceiptInputModel receiptInputModel)
        {
            var totalReceiptWithOutDiscont = receiptInputModel.ReceiptItems.Sum(x => x.ItemPrice * x.Quantity);
            receiptInputModel.Total = totalReceiptWithOutDiscont - receiptInputModel.Discount;
        }
    }
}
