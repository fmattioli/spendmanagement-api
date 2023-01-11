using System.Linq;

using static Spents.Application.Commands.AddSpent.AddSpentCommand;

namespace Spents.Application.Commands.AddSpent
{
    public class AddSpentInputModel
    {
        public string ReceiptName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<SpentDetailInputModel> ReceiptItems { get; set; } = null!;

        public AddSpentCommand ToCommand()
        {
            return new AddSpentCommand
            {
                ReceiptName = ReceiptName,
                ReceiptDate = ReceiptDate,
                ReceiptItems = ReceiptItems.Select(a => new SpentDetail
                {
                    Item = a.Item,
                    ItemPrice = a.ItemPrice,
                    Observation = a.Observation,
                    Quantity = a.Quantity,
                })
            };
        }
    }

    public class SpentDetailInputModel
    {
        public string Item { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; } = null!;
    }

}
