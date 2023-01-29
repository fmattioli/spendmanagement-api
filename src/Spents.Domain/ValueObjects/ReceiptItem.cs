namespace Spents.Domain.ValueObjects
{
    public class ReceiptItem
    {
        public ReceiptItem(string name, short quantity, decimal itemPrice, string observation)
        {
            Name = name;
            Quantity = quantity;
            ItemPrice = itemPrice;
            Observation = observation;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; }
    }
}
