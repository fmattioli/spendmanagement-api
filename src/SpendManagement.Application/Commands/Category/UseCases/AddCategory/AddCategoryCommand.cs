using MediatR;
using SpendManagement.Application.Commands.Category.InputModels;

namespace SpendManagement.Application.Commands.Category.UseCases.AddCategory
{
    public record AddCategoryCommand(CategoryInputModel AddCategoryInputModel) : IRequest<Unit>;
}
