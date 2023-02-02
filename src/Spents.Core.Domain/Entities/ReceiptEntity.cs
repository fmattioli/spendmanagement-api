using Spents.Domain.Entities;
using Spents.Domain.ValueObjects;

namespace Spents.Core.Domain.Entities
{
    public class ReceiptEntity : IReceipt
    {
        public ReceiptEntity(string receiptName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems)
        {
            Id = Guid.NewGuid();
            ReceiptName = receiptName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }

        public Guid Id { get ; set ; }
        public string ReceiptName { get ; set ; }
        public DateTime ReceiptDate { get ; set ; }
        public IEnumerable<ReceiptItem> ReceiptItems { get ; set ; }
    }
}
