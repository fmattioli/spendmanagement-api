using Web.Contracts.Category;
using Web.Contracts.Receipt;

namespace SpendManagement.Client.SpendManagementReadModel
{
    public interface ISpendManagementReadModelClient
    {
        Task<ReceiptResponse> GetReceiptAsync(Guid receiptId);

        Task<CategoryResponse> GetCategoryAsync(Guid categoryId);
    }
}
