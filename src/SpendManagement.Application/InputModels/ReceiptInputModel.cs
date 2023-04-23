namespace SpendManagement.Application.InputModels
{
    public class ReceiptInputModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemInputModel> ReceiptItems { get; set; } = null!;
    }
}
