namespace Spents.Core.Domain.ValueObjects
{
    public class ReceiptItemsDetailValueObject
    {
        public ReceiptItemsDetailValueObject(string itemName, short quantity, decimal itemPrice, decimal totalPrice, string observation)
        {
            Id = Guid.NewGuid();
            ItemName = itemName;
            Quantity = quantity;
            ItemPrice = itemPrice;
            Observation = observation;
            TotalPrice = totalPrice;
        }

        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
