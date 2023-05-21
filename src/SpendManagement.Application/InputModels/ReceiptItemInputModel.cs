namespace SpendManagement.Application.InputModels
{
    public record ReceiptItemInputModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CategoryInputModel Category { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get { return Quantity * ItemPrice; } }
        public string Observation { get; set; } = null!;
    }
}
