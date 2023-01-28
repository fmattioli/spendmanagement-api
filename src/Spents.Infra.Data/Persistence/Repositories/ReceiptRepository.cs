﻿using MongoDB.Driver;

using Spents.Domain.Entities;
using Spents.Domain.Interfaces;

namespace Spents.Infra.Data.Persistence.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IMongoCollection<Receipt> _receiptCollection;

        public ReceiptRepository(IMongoDatabase database)
        {
            _receiptCollection = database.GetCollection<Receipt>("receipts");
        }

        public async Task<Guid> AddReceipt(Receipt receipt)
        {
            await _receiptCollection.InsertOneAsync(receipt);
            return receipt.Id;
        }
    }
}
