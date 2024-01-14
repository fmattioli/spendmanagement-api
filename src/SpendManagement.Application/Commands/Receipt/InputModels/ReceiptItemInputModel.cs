namespace SpendManagement.Application.Commands.Receipt.InputModels
{
    public record ReceiptItemInputModel
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get { return Quantity * ItemPrice; } }
        public decimal ItemDiscount { get; set; }
        public string? Observation { get; set; }
    }
}
