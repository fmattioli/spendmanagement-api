using MediatR;

namespace Spents.Application.Commands.AddSpent
{
    public class AddSpentCommand : IRequest<Unit>
    {
        public string ReceiptName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<SpentDetail> ReceiptItems { get; set; } = null!;

        public class SpentDetail
        {
            public string Item { get; set; } = null!;
            public short Quantity { get; set; }
            public decimal ItemPrice { get; set; }
            public string Observation { get; set; } = null!;
        }
    }
}
