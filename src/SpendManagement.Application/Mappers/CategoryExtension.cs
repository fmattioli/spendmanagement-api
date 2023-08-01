using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Entities;
using Web.Contracts.Category;
using CommandMapper = SpendManagement.Contracts.V1.Commands.CategoryCommands;
namespace SpendManagement.Application.Mappers
{
    public static class CategoryExtension
    {
        public static CreateCategoryCommand ToCreateCategoryCommand(this CategoryInputModel categoryInputModel)
        {
            var categoryId = categoryInputModel.Id == Guid.Empty ? Guid.NewGuid() : categoryInputModel.Id;
            return new CreateCategoryCommand
            {
                Category = new Category(categoryId, categoryInputModel.Name)
            };
        }

        public static CommandMapper.DeleteCategoryCommand ToCommand(this Commands.Category.UseCases.DeleteCategory.DeleteCategoryCommand deleteCategoryCommand)
        {
            return new CommandMapper.DeleteCategoryCommand
            {
                Id = deleteCategoryCommand.Id,
                CommandCreatedDate = DateTime.UtcNow,
            };
        }

        public static UpdateCategoryCommand ToUpdateCategoryCommand(this CategoryResponse categoryInputModel)
        {
            var categoryId = categoryInputModel.Id == Guid.Empty ? Guid.NewGuid() : categoryInputModel.Id;
            return new UpdateCategoryCommand
            {
                Category = new Category(categoryId, categoryInputModel.Name ?? ""),
                CommandCreatedDate = DateTime.UtcNow,
            };
        }
    }
}
