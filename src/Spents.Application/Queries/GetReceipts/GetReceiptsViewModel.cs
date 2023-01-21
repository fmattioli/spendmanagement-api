namespace Spents.Application.Queries.GetReceipts
{
    public class GetReceiptsViewModel
    {
        public IEnumerable<string> EstablishmentNames { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public DateTime ReceiptDateFinal { get; set; }
    }
}
