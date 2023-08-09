﻿using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using Web.Contracts.Receipt;

namespace SpendManagement.Application.Mappers
{
    public static class ReceiptExtensions
    {
        public static CreateReceiptCommand ToCommand(this ReceiptInputModel receiptInputModel)
        {
            var receipt = new Receipt(receiptInputModel.Id == Guid.Empty ? Guid.NewGuid() : receiptInputModel.Id,
                                    receiptInputModel.EstablishmentName,
                                    receiptInputModel.ReceiptDate);

            var receiptItems = receiptInputModel
                                    .ReceiptItems
                                    .Select(x =>
                                        new ReceiptItem(
                                            x.Id == Guid.Empty ? Guid.NewGuid() : x.Id,
                                            x.ItemName,
                                            x.CategoryId,
                                            x.Quantity,
                                            x.ItemPrice,
                                            x.Observation));

            return new CreateReceiptCommand(receipt.Id.ToString(), receipt, receiptItems);
        }

        public static UpdateReceiptCommand ToCommand(this ReceiptResponse receiptResponse)
        {
            var receipt = new Receipt(receiptResponse.Id, receiptResponse.EstablishmentName, receiptResponse.ReceiptDate);
            var receptItems = receiptResponse.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, Guid.Empty, x.Quantity, x.ItemPrice, x.Observation));

            return new UpdateReceiptCommand(receipt.Id.ToString(), receipt, receptItems);
        }

        public static DeleteReceiptCommand ToCommand(this Commands.Receipt.UseCases.DeleteReceipt.DeleteReceiptCommand deleteReceiptCommand)
        {
            return new DeleteReceiptCommand(deleteReceiptCommand.Id.ToString(), deleteReceiptCommand.Id);
        }
    }
}