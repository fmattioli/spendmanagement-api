using Spents.Core.Entities;
using Spents.Core.ValueObjects;
using Spents.Events.Events.v1;

namespace Spents.Application.InputModels
{
    public class ReceiptInputModel
    {
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemsDetail> ReceiptItems { get; set; } = null!;

        public Receipt ToEntity() => new(
                EstablishmentName,
                ReceiptDate,
                ReceiptItems.Select(x => new ReceiptItems(x.Name, x.Quantity, x.ItemPrice, x.Observation))
            );

        public ReceiptCreatedEvent ToEvent(Guid messageKey)
        {
            return new ReceiptCreatedEvent(new ReceiptCreated
            {
                EstablishmentName = EstablishmentName,
                ReceiptDate = ReceiptDate,
                ReceiptItems = ReceiptItems.Select(x => new Events.Events.v1.ReceiptItemsDetail
                {
                    ItemPrice = x.ItemPrice,
                    Name = x.Name,
                    Observation = x.Observation,
                    Quantity = x.Quantity
                })
            }, messageKey.ToString());
        }
    }

    public class ReceiptItemsDetail
    {
        public string Name { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; } = null!;
    }

}
