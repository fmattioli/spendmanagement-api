namespace SpendManagement.Application.InputModels
{
    public class CategoryInputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
