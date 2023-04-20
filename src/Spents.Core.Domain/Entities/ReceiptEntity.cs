using Spents.Core.Domain.ValueObjects;

namespace Spents.Core.Domain.Entities
{
    public class ReceiptEntity
    {
        public ReceiptEntity(Guid id, string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems)
        {
            Id = id;
            EstablishmentName = establishmentName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }

        public Guid Id { get; set; }
        public string EstablishmentName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; }   

    }
}
