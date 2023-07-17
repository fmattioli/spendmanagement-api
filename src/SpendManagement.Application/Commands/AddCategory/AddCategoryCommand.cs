using MediatR;
using SpendManagement.Application.InputModels.Common;

namespace SpendManagement.Application.Commands.AddCategory
{
    public record AddCategoryCommand(CategoryInputModel AddCategoryInputModel) : IRequest<Unit>;
}
