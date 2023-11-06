namespace SpendManagement.Application.Validators
{
    public static class ValidationsErrorsMessages
    {
        public const string EstablishmentNameError = "EstablishmentName must have value";
        public const string ReceiptDateMinValueError = "ReceiptDate must have a valid date";
        public const string ReceiptItemsError = "ReceiptItems must have at least one item";
        public const string ReceiptItemsItemName = "The receipt item name cannot be null or empty";
        public const string ReceiptItemsItemQuantity = "Please inform at least one quantity";
        public const string ReceiptItemsItemPrice = "Please inform a valid price";
        public const string ReceiptItemsCategoryId = "Please inform a valid category";
    }
}
