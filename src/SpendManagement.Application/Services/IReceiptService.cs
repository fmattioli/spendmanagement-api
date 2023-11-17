namespace SpendManagement.Application.Services
{
    public interface IReceiptService
    {
        Task ValidateIfCategoryExistAsync(Guid categoryId);
    }
}
