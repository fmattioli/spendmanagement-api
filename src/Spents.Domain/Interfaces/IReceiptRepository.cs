using Spents.Domain.Entities;

namespace Spents.Domain.Interfaces
{
    public interface IReceiptRepository
    {
        Task<Guid> AddReceipt(Receipt receipt);
    }
}
