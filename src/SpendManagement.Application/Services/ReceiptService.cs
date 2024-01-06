using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Services
{
    public class ReceiptService(ISpendManagementReadModelClient spendManagementReadModelClient) : IReceiptService
    {
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient = spendManagementReadModelClient;

        public async Task ValidateIfCategoryExistAsync(Guid categoryId)
        {
            await _spendManagementReadModelClient
                .GetCategoriesAsync(categoryId);
        }
    }
}
