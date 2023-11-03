namespace SpendManagement.Application.Commands.Receipt.Services
{
    public interface IReceiptService
    {
        Task ValidateIfCategoryExistAsync(Guid categoryId);
    }
}
