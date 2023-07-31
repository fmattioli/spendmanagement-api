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
            return new CreateCategoryCommand
            {
                Category = new Category(categoryId, categoryInputModel.Name)
            };
        }

        public static DeleteCategoryCommand ToCommand(this Guid id)
        {
            return new DeleteCategoryCommand
            {
                Id = id,
            };
        }

        public static UpdateCategoryCommand ToUpdateCategoryCommand(this CategoryResponse categoryInputModel)
        {
            var categoryId = categoryInputModel.Id == Guid.Empty ? Guid.NewGuid() : categoryInputModel.Id;
            return new UpdateCategoryCommand
            {
                Category = new Category(categoryId, categoryInputModel.Name ?? "")
            };
        }
    }
}
