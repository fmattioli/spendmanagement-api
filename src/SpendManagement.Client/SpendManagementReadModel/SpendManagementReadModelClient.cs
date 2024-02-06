using Serilog;
using SpendManagement.Client.Configuration;
using SpendManagement.Client.Extensions;
using SpendManagement.Contracts.Exceptions;
using SpendManagement.Contracts.V1.Entities;
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
            var receipt = await GetAsync<PagedResult<ReceiptResponse>>("getReceipts", receiptId, "receiptIds").HandleExceptions("GetReceipt");

            if (receipt.TotalResults == 0)
            {
                throw new NotFoundException($"Invalid receipt provided, the receipt does not exists {receiptId}");
            }

            _logger.Information("Successfully got receipt: {@receiptId}", receiptId);

            return receipt;
        }
    }
}
