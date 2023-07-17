using SpendManagement.Application.InputModels.Common;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Entities;

namespace SpendManagement.Application.Mappers
{
    public static class CategoryExtension
    {
        public static CreateCategoryCommand ToCommand(this CategoryInputModel categoryInputModel)
        {
            return new CreateCategoryCommand
            {
                Category = new Category(categoryInputModel.Id, categoryInputModel.Name)
            };
        }
    }
}
