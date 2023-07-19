using MediatR;
using SpendManagement.Application.Commands.Category.InputModels;

namespace SpendManagement.Application.Commands.Category.UseCases.UpdateCategory
{
    public record UpdateCategoryCommand(UpdateCategoryInputModel UpdateCategoryInputModel) : IRequest<Unit>;
}
