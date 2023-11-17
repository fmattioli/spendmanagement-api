using MediatR;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Mappers;
using SpendManagement.Client.SpendManagementReadModel;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using SpendManagement.Client.Extensions;

namespace SpendManagement.Application.Commands.Category.UseCases.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICommandProducer _categoryProducer;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;
        private readonly IValidator<JsonPatchError> _validator;

        public UpdateCategoryCommandHandler(ICommandProducer receiptProducer,
            ISpendManagementReadModelClient spendManagementReadModelClient,
            IValidator<JsonPatchError> validator)
        {
            _categoryProducer = receiptProducer;
            _spendManagementReadModelClient = spendManagementReadModelClient;
            _validator = validator;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await
                _spendManagementReadModelClient
                .GetCategoryAsync(request.UpdateCategoryInputModel.Id);

            request
                .UpdateCategoryInputModel
                .CategoryPatchDocument
                .ApplyTo(category, JsonPatchExtension.HandlePatchErrors(_validator));

            await _categoryProducer.ProduceCommandAsync(category.ToUpdateCategoryCommand());
            return Unit.Value;
        }
    }
}
