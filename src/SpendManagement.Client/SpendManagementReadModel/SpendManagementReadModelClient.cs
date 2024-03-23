using Serilog;
using SpendManagement.Client.Configuration;
using SpendManagement.Client.Extensions;
using SpendManagement.WebContracts.Category;
using SpendManagement.WebContracts.Common;
using SpendManagement.WebContracts.Exceptions;
using SpendManagement.WebContracts.Receipt;

namespace SpendManagement.Client.SpendManagementReadModel
{
    public class SpendManagementReadModelClient(IApiConfiguration configuration, HttpClient httpClient, ILogger logger) : BaseClient(httpClient, configuration), ISpendManagementReadModelClient
    {
        private readonly ILogger _logger = logger;

        public async Task<PagedResult<CategoryResponse>> GetCategoriesAsync(Guid categoryId)
        {
            var category = await GetAsync<PagedResult<CategoryResponse>>("getCategories", categoryId, "categoryIds")
                .HandleExceptions("GetCategories");

            if(category.TotalResults == 0)
            {
                throw new NotFoundException($"Invalid categoryId provided, the category does not exists {category}");
            }

            _logger.Information("Successfully got Category: {@categoryId}", categoryId);

            return category;
        }

        public async Task<PagedResult<ReceiptResponse>> GetReceiptAsync(Guid receiptId)
        {
            var receipt = await GetAsync<PagedResult<ReceiptResponse>>("getVariableReceipts", receiptId, "receiptIds").HandleExceptions("GetReceipt");

            if (receipt.TotalResults == 0)
            {
                throw new NotFoundException($"Invalid receipt provided, the receipt does not exists {receiptId}");
            }

            _logger.Information("Successfully got receipt: {@receiptId}", receiptId);

            return receipt;
        }

        public async Task<PagedResult<RecurringReceiptResponse>> GetRecurringReceiptAsync(Guid receiptId)
        {
            var receipt = await GetAsync<PagedResult<RecurringReceiptResponse>>("getRecurringReceipts", receiptId, "recurringReceiptIds").HandleExceptions("GetRecurringReceipt");

            if (receipt.TotalResults == 0)
            {
                throw new NotFoundException($"Invalid recurring receipt provided, the recurring receipt does not exists {receiptId}");
            }

            _logger.Information("Successfully got recuring receipt: {@receiptId}", receiptId);

            return receipt;
        }
    }
}
