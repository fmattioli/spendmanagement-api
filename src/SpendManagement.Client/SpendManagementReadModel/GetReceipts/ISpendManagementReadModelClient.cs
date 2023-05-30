using Web.Contracts.UseCases.Common;

namespace SpendManagement.Client.SpendManagementReadModel.GetReceipts
{
    public interface ISpendManagementReadModelClient
    {
        Task<ReceiptResponse?> GetReceipt(Guid receiptId);
    }
}
