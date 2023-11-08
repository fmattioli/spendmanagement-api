using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Entities;
using Web.Contracts.Category;

namespace SpendManagement.Application.Mappers
{
    public static class CategoryExtension
    {
        public static CreateCategoryCommand ToCreateCategoryCommand(this CategoryInputModel categoryInputModel)
        {
            var categoryId = categoryInputModel.Id == Guid.Empty ? Guid.NewGuid() : categoryInputModel.Id;
            var category = new Category(categoryId, categoryInputModel.Name);

            return new CreateCategoryCommand(category);
        }

        public static DeleteCategoryCommand ToCommand(this Commands.Category.UseCases.DeleteCategory.DeleteCategoryCommand deleteCategoryCommand)
        {
            return new DeleteCategoryCommand(deleteCategoryCommand.Id);
        }

        public static UpdateCategoryCommand ToUpdateCategoryCommand(this CategoryResponse categoryInputModel)
        {
            var categoryId = categoryInputModel.Id == Guid.Empty ? Guid.NewGuid() : categoryInputModel.Id;
            var category = new Category(categoryId, categoryInputModel.Name ?? "");

            return new UpdateCategoryCommand(category);
        }
    }
}
