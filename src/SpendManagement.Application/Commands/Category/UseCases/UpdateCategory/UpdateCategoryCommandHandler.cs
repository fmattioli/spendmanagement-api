using MediatR;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using SpendManagement.Application.Extensions;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel.GetReceipts;
using FluentValidation.Results;
using SpendManagement.Application.Mappers;
using Serilog;

namespace SpendManagement.Application.Commands.Category.UseCases.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICommandProducer _categoryProducer;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;
        private readonly ILogger _logger;

        public UpdateCategoryCommandHandler(ICommandProducer receiptProducer,
            ISpendManagementReadModelClient spendManagementReadModelClient,
            ILogger logger)
        {
            _categoryProducer = receiptProducer;
            _spendManagementReadModelClient = spendManagementReadModelClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _spendManagementReadModelClient
                .GetCategoryAsync(request.UpdateCategoryInputModel.Id) ?? throw new NotFoundException("Any category was found");

            var validationResult = new ValidationResult();

            request.UpdateCategoryInputModel
                .CategoryPatchDocument
                .ApplyTo(category, JsonPatchExtension.HandlePatchErrors(validationResult));

            if (!validationResult.IsValid)
            {
                _logger.Error(
                    "Invalid json provided.",
                    () => new
                    {
                        validationResult.Errors
                    });

                throw new JsonPatchInvalidException(string.Join(",", validationResult.Errors));
            }

            await _categoryProducer.ProduceCommandAsync(category.ToUpdateCategoryCommand());
            return Unit.Value;
        }
    }
}
