namespace SpendManagement.Application.InputModels
{
    public class AddCategoryInputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
