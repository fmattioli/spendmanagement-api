using SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels;
using SpendManagement.Application.Commands.Receipt.VariableReceipt.UseCases.DeleteReceipt;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.WebContracts.Receipt;

namespace SpendManagement.Application.Mappers
{
    public static class ReceiptExtensions
    {
        public static CreateReceiptCommand ToCommand(this ReceiptInputModel receiptInputModel)
        {
            var receipt = new Receipt(receiptInputModel.Id == Guid.Empty ? Guid.NewGuid() : receiptInputModel.Id,
                                    receiptInputModel.CategoryId,
                                    receiptInputModel.EstablishmentName!,
                                    receiptInputModel.ReceiptDate,
                                    receiptInputModel.Discount,
                                    receiptInputModel.Total);

            var receiptItems = receiptInputModel
                                    .ReceiptItems
                                    .Select(x =>
                                        new ReceiptItem(
                                            x.Id == Guid.Empty ? Guid.NewGuid() : x.Id,
                                            x.ItemName,
                                            x.Quantity,
                                            x.ItemPrice,
                                            x.Observation!,
                                            x.ItemDiscount,
                                            x.TotalPrice));

            return new CreateReceiptCommand(receipt, receiptItems);
        }

        public static UpdateReceiptCommand ToCommand(this ReceiptResponse receiptResponse)
        {
            var receipt = new Receipt(receiptResponse.Id, 
                receiptResponse.CategoryId, 
                receiptResponse.EstablishmentName, 
                receiptResponse.ReceiptDate, 
                receiptResponse.Discount,
                receiptResponse.Total);

            var receptItems = receiptResponse.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.Quantity, x.ItemPrice, x.Observation, x.ItemDiscount,x.TotalPrice));

            return new UpdateReceiptCommand(receipt, receptItems);
        }

        public static DeleteReceiptCommand ToCommand(this DeleteVariableReceiptCommand deleteReceiptCommand)
        {
            return new DeleteReceiptCommand(deleteReceiptCommand.Id);
        }
    }
}