using Serilog;
using SpendManagement.Client.Configuration;
using Web.Contracts.Category;
using Web.Contracts.Receipt;

namespace SpendManagement.Client.SpendManagementReadModel.GetReceipts
{
    public class SpendManagementReadModelClient : BaseClient, ISpendManagementReadModelClient
    {
        private readonly ILogger _logger;
        public SpendManagementReadModelClient(IApiConfiguration configuration, HttpClient httpClient, ILogger logger) : base(httpClient, configuration)
        {
            _logger = logger;
        }

        public async Task<CategoryResponse> GetCategoryAsync(Guid categoryId)
        {
            var category = await GetByIdAsync<CategoryResponse>("getCategory", categoryId);

            _logger.Information(
                    "Successfully got Category",
                    () => new
                    {
                        category
                    });

            return category;
        }

        public async Task<ReceiptResponse> GetReceiptAsync(Guid receiptId)
        {
            var receipt = await GetByIdAsync<ReceiptResponse>("getReceipt", receiptId);

            _logger.Information(
                    "Successfully got Receipt",
                    () => new
                    {
                        receipt
                    });

            return receipt;
        }
    }
}
