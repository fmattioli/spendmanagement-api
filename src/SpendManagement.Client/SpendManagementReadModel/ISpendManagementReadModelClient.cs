using SpendManagement.WebContracts.Category;
using SpendManagement.WebContracts.Common;
using SpendManagement.WebContracts.Receipt;

namespace SpendManagement.Client.SpendManagementReadModel
{
    public interface ISpendManagementReadModelClient
    {
        Task<PagedResult<ReceiptResponse>> GetReceiptAsync(Guid receiptId);

        Task<PagedResult<CategoryResponse>> GetCategoriesAsync(Guid categoryId);
    }
}
