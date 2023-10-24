using MediatR;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using SpendManagement.Application.Extensions;
using SpendManagement.Application.Producers;
using FluentValidation.Results;
using SpendManagement.Application.Mappers;
using Serilog;
using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Commands.Category.UseCases.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICommandProducer _categoryProducer;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;
        private readonly ILogger _logger;
        private readonly ValidationResult _validationResult;

        public UpdateCategoryCommandHandler(ICommandProducer receiptProducer,
            ISpendManagementReadModelClient spendManagementReadModelClient,
            ILogger logger,
            ValidationResult validationResult)
        {
            _categoryProducer = receiptProducer;
            _spendManagementReadModelClient = spendManagementReadModelClient;
            _logger = logger;
            _validationResult = validationResult;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _spendManagementReadModelClient
                .GetCategoryAsync(request.UpdateCategoryInputModel.Id) ?? throw new NotFoundException("Any category was found");

            request
                .UpdateCategoryInputModel
                .CategoryPatchDocument
                .ApplyTo(category, JsonPatchExtension.HandlePatchErrors(_validationResult));

            if (!_validationResult.IsValid)
            {
                _logger.Error("Invalid json provided.: {@Errors}", _validationResult.Errors);
                throw new JsonPatchInvalidException(string.Join(",", _validationResult.Errors));
            }

            await _categoryProducer.ProduceCommandAsync(category.ToUpdateCategoryCommand());
            return Unit.Value;
        }
    }
}
