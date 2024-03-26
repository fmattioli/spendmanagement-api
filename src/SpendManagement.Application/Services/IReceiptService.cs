using SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels;

namespace SpendManagement.Application.Services
{
    public interface IReceiptService
    {
        Task ValidateIfCategoryExistAsync(Guid categoryId);
        ReceiptInputModel CalculateReceiptTotals(ReceiptInputModel receiptInputModel);
    }
}
