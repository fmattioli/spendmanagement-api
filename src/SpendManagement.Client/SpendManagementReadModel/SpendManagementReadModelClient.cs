using Serilog;
using SpendManagement.Client.Configuration;
using SpendManagement.Client.Extensions;
using SpendManagement.WebContracts.Category;
using SpendManagement.WebContracts.Common;
using SpendManagement.WebContracts.Receipt;

namespace SpendManagement.Client.SpendManagementReadModel
{
    public class SpendManagementReadModelClient(IApiConfiguration configuration, HttpClient httpClient, ILogger logger) : BaseClient(httpClient, configuration), ISpendManagementReadModelClient
    {
        private readonly ILogger _logger = logger;

        public async Task<PagedResult<CategoryResponse>> GetCategoriesAsync(Guid categoryId)
        {
            var category = await GetByIdAsync<PagedResult<CategoryResponse>>("getCategories", categoryId, "categoryIds").HandleExceptions("GetCategories");

            _logger.Information("Successfully got Category: {@categoryId}", categoryId);

            return category;
        }

        public async Task<PagedResult<ReceiptResponse>> GetReceiptAsync(Guid receiptId)
        {
            var receipt = await GetByIdAsync<PagedResult<ReceiptResponse>>("getReceipt", receiptId, "receiptIds").HandleExceptions("GetReceipt");

            _logger.Information("Successfully got receipt: {@receiptId}", receiptId);

            return receipt;
        }
    }
}
