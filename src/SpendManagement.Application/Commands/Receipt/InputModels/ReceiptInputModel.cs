using Newtonsoft.Json;

namespace SpendManagement.Application.Commands.Receipt.InputModels
{
    public class ReceiptInputModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string? EstablishmentName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemInputModel> ReceiptItems { get; set; } = Enumerable.Empty<ReceiptItemInputModel>();
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
