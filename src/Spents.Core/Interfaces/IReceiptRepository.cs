using Spents.Core.Entities;

namespace Spents.Core.Interfaces
{
    public interface IReceiptRepository
    {
        Task<Guid> AddReceipt(Receipt receipt);
    }
}
