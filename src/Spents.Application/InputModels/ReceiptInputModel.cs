using Spents.Core.Domain.Entities;
using Spents.Events.v1;
using Spents.Events.Body;
using Spents.Domain.Entities;
using Spents.Domain.ValueObjects;
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
        private string MessageKey { get { return Id.ToString(); } }

        public ReceiptEntity ToEntity() => new(
                Id,
                EstablishmentName,
                ReceiptDate,
                ReceiptItemsDetail.Select(x => new ReceiptItemsDetailValueObject(
                    x.ItemName, 
                    x.Quantity, 
                    x.ItemPrice, 
                    x.TotalPrice,
                    x.Observation))
            );

        public ReceiptCreatedEvent ToEvent(ReceiptEntity receiptEntity)
        {
            return new ReceiptCreatedEvent(
                new MessageBody<Receipt>(
                    new Receipt(
                        receiptEntity.Id,
                        receiptEntity.EstablishmentName,
                        receiptEntity.ReceiptDate,
                        receiptEntity.ReceiptItems
                        .Select(x => new ReceiptItem(
                            x.Id, 
                            x.ItemName, 
                            x.Quantity, 
                            x.ItemPrice,
                            x.TotalPrice,
                            x.Observation))
                        )
                    ),
                MessageKey);
        }
    }
}
