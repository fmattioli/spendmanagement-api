using MongoDB.Bson.Serialization.Attributes;
using Spents.Core.Domain.ValueObjects;

namespace Spents.Core.Domain.Entities
{
    public class ReceiptEntity
    {
        public ReceiptEntity(Guid id,string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItemsDetailValueObject> receiptItems)
        {
            Id = id;
            EstablishmentName = establishmentName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string EstablishmentName { get; set; }
        public virtual DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemsDetailValueObject> ReceiptItems { get; set; }
    }
}
