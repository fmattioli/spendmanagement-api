namespace SpendManagement.Application.InputModels
{
    public class AddReceiptInputModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<AddReceiptItemInputModel> ReceiptItems { get; set; } = null!;
    }
}
