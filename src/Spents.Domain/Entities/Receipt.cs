using Spents.Domain.ValueObjects;

namespace Spents.Domain.Entities
{
    public class Receipt
    {
        public Receipt(string receiptName, DateTime receiptDate, IEnumerable<ReceiptItems> receiptItems)
        {
            ReceiptName = receiptName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string ReceiptName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItems> ReceiptItems { get; set; }
    }
}
