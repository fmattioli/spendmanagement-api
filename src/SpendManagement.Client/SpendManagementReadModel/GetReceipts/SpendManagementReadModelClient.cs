using SpendManagement.Client.Configuration;
using Web.Contracts.Receipt;

namespace SpendManagement.Client.SpendManagementReadModel.GetReceipts
{
    public class SpendManagementReadModelClient : BaseClient, ISpendManagementReadModelClient
    {
        public SpendManagementReadModelClient(IApiConfiguration configuration, HttpClient httpClient ) : base(httpClient, configuration)
        {
        }

        public async Task<ReceiptResponse?> GetReceiptAsync(Guid receiptId)
        {
            var retorno = await GetByIdAsync<ReceiptResponse>("getReceipt", receiptId);
            return retorno;
        }
    }
}
