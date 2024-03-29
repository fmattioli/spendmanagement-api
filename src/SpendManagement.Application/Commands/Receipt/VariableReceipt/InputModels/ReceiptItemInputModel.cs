﻿namespace SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels
{
    public record ReceiptItemInputModel
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ItemDiscount { get; set; }
        public string? Observation { get; set; }
    }
}
