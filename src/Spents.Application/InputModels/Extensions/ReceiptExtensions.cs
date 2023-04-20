using Spents.Contracts.Documents;
using Spents.Core.Domain.Entities;
using Spents.Events.v1;

namespace Spents.Application.InputModels.Extensions
{
    public static class ReceiptExtensions
    {
        public static ReceiptEntity ToEntity(this ReceiptInputModel receiptInputModel) => new(
                receiptInputModel.Id,
                receiptInputModel.EstablishmentName,
                receiptInputModel.ReceiptDate,
                receiptInputModel.ReceiptItems.Select(x => new Core.Domain.ValueObjects.ReceiptItem(x.ItemName, x.Quantity, x.ItemPrice, x.TotalPrice, x.Observation))
            );
        public static ReceiptEvent<ReceiptEntity> ToReceiptCreatedEvent(this ReceiptEntity receiptEntity)
        {
            return new ReceiptEvent<ReceiptEntity>(receiptEntity.Id, receiptEntity, Events.v1.ValueObjects.EventType.Created, "ReceiptCreated");
        }

        public static ReceiptDocument ToReceiptDocument(this ReceiptEntity receiptEntity)
        {
            return new ReceiptDocument
            {
                Id = receiptEntity.Id,
                EstablishmentName = receiptEntity.EstablishmentName,
                ReceiptDate = receiptEntity.ReceiptDate,
                ReceiptItems = receiptEntity.ReceiptItems.Select(x => new Contracts.Documents.ReceiptItem
                {
                    Id = x.Id,
                    ItemName = x.ItemName,
                    Quantity = x.Quantity,
                    ItemPrice = x.ItemPrice,
                    Observation = x.Observation,
                    TotalPrice = x.TotalPrice
                })
            };
        }
    }
}