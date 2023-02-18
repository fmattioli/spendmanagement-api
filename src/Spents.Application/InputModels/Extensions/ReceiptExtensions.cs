using Spents.Core.Domain.Entities;
using Spents.Core.Domain.ValueObjects;
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
                receiptInputModel.ReceiptItemsDetail.Select(x => new ReceiptItemsDetailValueObject(
                    x.ItemName,
                    x.Quantity,
                    x.ItemPrice,
                    x.TotalPrice,
                    x.Observation))
            );
        public static ReceiptEventCreated ToCreatedEvent(this ReceiptEntity receiptEntity)
        {
            return new ReceiptEventCreated(new Receipt(
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
                );
        }
    }
}
