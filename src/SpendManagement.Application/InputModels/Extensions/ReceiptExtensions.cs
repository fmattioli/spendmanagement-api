using SpendManagement.Contracts.V1.Base;
using SpendManagement.Contracts.V1.Commands;

namespace SpendManagement.Application.InputModels.Extensions
{
    public static class ReceiptExtensions
    {
        public static CreateReceiptCommand ToCommand(this ReceiptInputModel receiptInputModel)
        {
            return new CreateReceiptCommand
            {
                Id = receiptInputModel.Id,
                EstablishmentName = receiptInputModel.EstablishmentName,
                ReceiptDate = receiptInputModel.ReceiptDate,
                ReceiptItems = receiptInputModel.ReceiptItems.Select(x => new ReceiptItem
                {
                    Id = x.Id,
                    ItemName = x.ItemName,
                    ItemPrice = x.ItemPrice,
                    Observation = x.Observation,
                    Quantity = x.Quantity,
                }),
            };
        }
    }
}