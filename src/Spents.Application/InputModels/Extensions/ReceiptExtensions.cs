using Spents.Core.Domain.Entities;
using Spents.Core.Domain.ValueObjects;
using Spents.Domain.Documents;
using Spents.Domain.Entities;
using Spents.Domain.ValueObjects;
using Spents.Events.v1;

namespace Spents.Application.InputModels.Extensions
{
    public static class ReceiptExtensions
    {
        public static ReceiptEntity ToEntity(this ReceiptInputModel receiptInputModel) => new(
                receiptInputModel.Id,
                receiptInputModel.EstablishmentName,
                receiptInputModel.ReceiptDate,
                receiptInputModel.ReceiptItems.Select(x => new ReceiptItem
                {
                    Id = Guid.NewGuid(),
                    ItemName = x.ItemName,
                    ItemPrice = x.ItemPrice,
                    Observation = x.Observation,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice,
                })
            );
        public static ReceiptEvent<Receipt> ToReceiptCreatedEvent(this ReceiptEntity receiptEntity)
        {
            return new ReceiptEvent<Receipt>(receiptEntity.Id, receiptEntity, Events.v1.ValueObjects.EventType.Created, "ReceiptCreated");
        }

        public static ReceiptDocument ToReceiptDocument(this ReceiptEntity receiptEntity)
        {
            return new ReceiptDocument
            {
                Receipt = receiptEntity
            };
        }
    }
}
