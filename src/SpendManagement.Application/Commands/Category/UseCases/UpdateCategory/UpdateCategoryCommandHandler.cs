using MediatR;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Mappers;
using SpendManagement.Client.SpendManagementReadModel;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using SpendManagement.Client.Extensions;

namespace SpendManagement.Application.Commands.Category.UseCases.UpdateCategory
{
    public class UpdateCategoryCommandHandler(ICommandProducer receiptProducer,
        ISpendManagementReadModelClient spendManagementReadModelClient,
        IValidator<JsonPatchError> validator) : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICommandProducer _categoryProducer = receiptProducer;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient = spendManagementReadModelClient;
        private readonly IValidator<JsonPatchError> _validator = validator;

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryPagedResult = await
                _spendManagementReadModelClient
                .GetCategoriesAsync(request.UpdateCategoryInputModel.Id);

            var category = categoryPagedResult.Results.FirstOrDefault();
            
            if(category is not null)
            {
                request
                .UpdateCategoryInputModel
                .CategoryPatchDocument
                .ApplyTo(category, JsonPatchExtension.HandlePatchErrors(_validator));

                await _categoryProducer.ProduceCommandAsync(category.ToUpdateCategoryCommand());
            }
            
            return Unit.Value;
        }
    }
}
