using Newtonsoft.Json;

namespace SpendManagement.Application.Commands.Receipt.InputModels
{
    public record ReceiptItemInputModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get { return Quantity * ItemPrice; } }
        public string Observation { get; set; } = null!;
    }
}
