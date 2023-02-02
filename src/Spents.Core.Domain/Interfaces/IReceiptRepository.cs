using Spents.Core.Domain.Entities;

namespace Spents.Core.Domain.Interfaces
{
    public interface IReceiptRepository
    {
        Task<Guid> AddReceipt(ReceiptEntity receipt);
    }
}
