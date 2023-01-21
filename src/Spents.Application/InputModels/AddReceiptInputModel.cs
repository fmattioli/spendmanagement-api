using Spents.Core.Entities;
using Spents.Core.ValueObjects;
using Spents.Events.Events.v1;

namespace Spents.Application.InputModels
{
    public class AddReceiptInputModel
    {
        public string ReceiptName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemsDetail> ReceiptItems { get; set; } = null!;


        public Receipt ToEntity() => new(
                ReceiptName,
                ReceiptDate,
                ReceiptItems.Select(x => new ReceiptItems(x.Name, x.Quantity, x.ItemPrice, x.Observation))
            );

        public ReceiptCreatedEvent ToEvent() => new ReceiptCreatedEvent(new ReceiptCreated
        {
            ReceiptName = ReceiptName,
            ReceiptDate = ReceiptDate,
            ReceiptItems = ReceiptItems.Select(x => new Events.Events.v1.ReceiptItemsDetail
            {
                ItemPrice = x.ItemPrice,
                Observation = x.Observation,
                Name = x.Name,
                Quantity = x.Quantity,
            })
        });
    }

    public class ReceiptItemsDetail
    {
        public string Name { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; } = null!;
    }


}
