using MongoDB.Driver;
using Spents.Core.Domain.Entities;
using Spents.Core.Domain.Interfaces;

namespace Spents.Infra.Data.Persistence.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IMongoCollection<ReceiptEntity> _receiptCollection;

        public ReceiptRepository(IMongoDatabase database)
        {
            _receiptCollection = database.GetCollection<ReceiptEntity>("receipts");
        }

        public async Task<Guid> AddReceipt(ReceiptEntity receipt)
        {
            await _receiptCollection.InsertOneAsync(receipt);
            return receipt.Id;
        }
    }
}
