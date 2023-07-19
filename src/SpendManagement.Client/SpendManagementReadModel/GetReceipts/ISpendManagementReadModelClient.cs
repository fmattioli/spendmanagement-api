using Web.Contracts.Receipt;

namespace SpendManagement.Client.SpendManagementReadModel.GetReceipts
{
    public interface ISpendManagementReadModelClient
    {
        Task<ReceiptResponse?> GetReceiptAsync(Guid receiptId);
    }
}
