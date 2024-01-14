namespace SpendManagement.Application.Validators
{
    public static class ValidationsErrorsMessages
    {
        public const string CategoryIdNameError = "CategoryId must have a valid guid";
        public const string EstablishmentNameError = "EstablishmentName must have value";
        public const string ReceiptDateMinValueError = "ReceiptDate must have a valid date";
        public const string ReceiptItemsError = "ReceiptItems must have at least one item";
        public const string ReceiptItemsItemName = "The receipt item name cannot be null or empty";
        public const string ReceiptItemsItemQuantity = "Please inform at least one quantity";
        public const string ReceiptItemsItemPrice = "Please inform a valid price";
        public const string DiscountFilledOnMoreThanOneField = "Please, you cannot provide an item discount and a total discount together (ItemDiscount and Discount fields). Choose just one of them. You can enter the discount for each item through the ItemDiscount field or you can enter the discount directly on the receipt through the Discount field.";
    }
}
