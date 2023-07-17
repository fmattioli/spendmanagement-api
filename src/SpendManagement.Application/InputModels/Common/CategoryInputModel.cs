namespace SpendManagement.Application.InputModels.Common
{
    public class CategoryInputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
