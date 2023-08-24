namespace SpendManagement.Application.Commands.Receipt.Services
{
    public interface IReceiptService
    {
        Task ValidateIfCategoriesExists(Guid categoryId);
    }
}
