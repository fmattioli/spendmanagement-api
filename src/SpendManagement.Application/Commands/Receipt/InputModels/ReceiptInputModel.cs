using Newtonsoft.Json;

namespace SpendManagement.Application.Commands.Receipt.InputModels
{
    public class ReceiptInputModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemInputModel> ReceiptItems { get; set; } = null!;
    }
}
