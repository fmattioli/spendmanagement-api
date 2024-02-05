using SpendManagement.Application.Commands.Receipt.InputModels;

namespace SpendManagement.Application.Services
{
    public interface IReceiptService
    {
        Task ValidateIfCategoryExistAsync(Guid categoryId);
        ReceiptInputModel CalculateReceiptTotals(ReceiptInputModel receiptInputModel);
    }
}
