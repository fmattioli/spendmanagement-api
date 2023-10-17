using MediatR;
using SpendManagement.Application.Commands.Category.InputModels;

namespace SpendManagement.Application.Commands.Category.UseCases.AddCategory
{
    public record AddCategoryRequestCommand(CategoryInputModel AddCategoryInputModel) : IRequest<Guid>;
}
