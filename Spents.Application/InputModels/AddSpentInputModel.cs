namespace Spents.Application.InputModels
{
    public class AddSpentInputModel
    {
        public string Name { get; set; }
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string Observation { get; set; }
    }
}
