namespace Spents.Core.Entities
{
    public class ReceiptItems
    {
        public ReceiptItems(string name, short quantity, decimal itemPrice, string observation)
        {
            Name = name;
            Quantity = quantity;
            ItemPrice = itemPrice;
            Observation = observation;
        }

        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; }
    }
}
