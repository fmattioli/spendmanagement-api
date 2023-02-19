using MongoDB.Bson.Serialization.Attributes;
using Spents.Domain.Entities;
using Spents.Domain.ValueObjects;

namespace Spents.Core.Domain.Entities
{
    public class ReceiptEntity : Receipt
    {
        public ReceiptEntity(Guid id, string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems)
        {
            Id = id;
            EstablishmentName = establishmentName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }
    }
}
