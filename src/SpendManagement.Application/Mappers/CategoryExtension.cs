using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Entities;

namespace SpendManagement.Application.Mappers
{
    public static class CategoryExtension
    {
        public static CreateCategoryCommand ToCommand(this CategoryInputModel categoryInputModel)
        {
            var categoryId = categoryInputModel.Id == Guid.Empty ? Guid.NewGuid() : categoryInputModel.Id;
            return new CreateCategoryCommand
            {
                Category = new Category(categoryId, categoryInputModel.Name)
            };
        }
    }
}
