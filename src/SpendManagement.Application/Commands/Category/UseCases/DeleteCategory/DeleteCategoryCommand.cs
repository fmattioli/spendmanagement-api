using MediatR;

namespace SpendManagement.Application.Commands.Category.UseCases.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;
}
