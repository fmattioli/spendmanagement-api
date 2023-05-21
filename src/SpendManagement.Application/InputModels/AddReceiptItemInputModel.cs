namespace SpendManagement.Application.InputModels
{
    public record AddReceiptItemInputModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public AddCategoryInputModel Category { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get { return Quantity * ItemPrice; } }
        public string Observation { get; set; } = null!;
    }
}
