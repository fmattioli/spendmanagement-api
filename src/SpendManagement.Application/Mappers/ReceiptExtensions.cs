using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using Web.Contracts.Receipt;

namespace SpendManagement.Application.Mappers
{
    public static class ReceiptExtensions
    {
        public static CreateReceiptCommand ToCommand(this ReceiptInputModel receiptInputModel)
        {
            return new CreateReceiptCommand
            {
                Receipt = new Receipt(
                                    receiptInputModel.Id == Guid.Empty ? Guid.NewGuid() : receiptInputModel.Id,
                                    receiptInputModel.EstablishmentName, 
                                    receiptInputModel.ReceiptDate),
                ReceiptItems = receiptInputModel
                                    .ReceiptItems
                                    .Select(x =>
                                        new ReceiptItem(
                                            x.Id == Guid.Empty ? Guid.NewGuid () : x.Id, 
                                            x.ItemName,
                                            x.CategoryId,
                                            x.Quantity,
                                            x.ItemPrice,
                                            x.Observation))
            };
        }

        public static UpdateReceiptCommand ToCommand(this ReceiptResponse receiptResponse)
        {
            return new UpdateReceiptCommand
            {
                CommandCreatedDate = DateTime.Now,
                Receipt = new Receipt(receiptResponse.Id, receiptResponse.EstablishmentName, receiptResponse.ReceiptDate),
                ReceiptItems = receiptResponse.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, Guid.Empty, x.Quantity, x.ItemPrice, x.Observation))
            };
        }
    }
}