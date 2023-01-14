using MongoDB.Driver;

using Spents.Core.Entities;
using Spents.Core.Interfaces;

namespace Spents.Infra.Data.Persistence.Repositories
{
    public class SpentRepository : ISpentRepository
    {
        private readonly IMongoCollection<Receipt> _receiptCollection;

        public SpentRepository(IMongoDatabase database)
        {
            _receiptCollection = database.GetCollection<Receipt>("receipts");
        }

        public async Task<Guid> AddSpent(Receipt receipt)
        {
            await _receiptCollection.InsertOneAsync(receipt);
            return receipt.Id;
        }
    }
}
