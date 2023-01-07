namespace Spents.Core.Entities
{
    public class Receipt
    {
        public Receipt(string receiptName, DateTime receiptDate, IEnumerable<ReceiptItems> receiptItems)
        {
            ReceiptName = receiptName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }

        public string ReceiptName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItems> ReceiptItems { get; set; }
    }
}
