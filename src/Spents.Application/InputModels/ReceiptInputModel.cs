using Spents.Core.Domain.Entities;
using Spents.Core.Domain.ValueObjects;

namespace Spents.Application.InputModels
{
    public class ReceiptInputModel
    {
        public ReceiptInputModel()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemsDetailInputModel> ReceiptItemsDetail { get; set; } = null!;
    }
}
