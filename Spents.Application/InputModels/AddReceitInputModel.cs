using Spents.Core.Entities;
using Spents.Core.ValueObjects;
namespace Spents.Application.InputModels
{
    public class AddReceitInputModel
    {
        public string ReceiptName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<SpentDetailInputModel> ReceiptItems { get; set; } = null!;


        public Receipt ToEntity() => new(
                ReceiptName,
                ReceiptDate,
                ReceiptItems.Select(x => new ReceiptItems(x.Name, x.Quantity, x.ItemPrice, x.Observation))
            );
    }

    public class SpentDetailInputModel
    {
        public string Name { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; } = null!;
    }


}
