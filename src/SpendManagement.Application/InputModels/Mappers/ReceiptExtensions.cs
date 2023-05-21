using SpendManagement.Contracts.V1.Base;
using SpendManagement.Contracts.V1.Commands;
using SpendManagement.Contracts.V1.Entities;

namespace SpendManagement.Application.InputModels.Extensions
{
    public static class ReceiptExtensions
    {
        public static CreateReceiptCommand ToCommand(this ReceiptInputModel receiptInputModel)
        {
            return new CreateReceiptCommand
            {
                Receipt = new Receipt(receiptInputModel.Id, receiptInputModel.EstablishmentName, receiptInputModel.ReceiptDate),
                ReceiptItems = receiptInputModel.ReceiptItems.Select(x => new ReceiptItem(
                    x.Id, x.ItemName, 
                    new Category(x.Category.Id, x.Category.Name), 
                    x.Quantity, 
                    x.ItemPrice, 
                    x.Observation))
            };
        }
    }
}