using Spents.Contracts.V1.Base;
using Spents.Contracts.V1.Commands;

namespace Spents.Application.InputModels.Extensions
{
    public static class ReceiptExtensions
    {
        public static CreateReceiptCommand ToCommand(this ReceiptInputModel receiptInputModel) => new(receiptInputModel.Id, 
            receiptInputModel.ReceiptDate, 
            receiptInputModel.EstablishmentName,
            receiptInputModel
            .ReceiptItems
            .Select(x => new ReceiptItem(Guid.NewGuid(), x.ItemName, x.Quantity, x.ItemPrice, x.Observation)));
    }
}