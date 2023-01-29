using Spents.Domain.ValueObjects;

namespace Spents.Domain.Entities
{
    public class Receipt
    {
        public Receipt(string receiptName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems)
        {
            ReceiptName = receiptName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string ReceiptName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; }
    }
}
