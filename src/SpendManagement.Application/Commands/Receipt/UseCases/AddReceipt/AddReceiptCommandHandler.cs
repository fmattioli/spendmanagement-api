using FluentValidation;
using MediatR;
using Serilog;

using SpendManagement.Application.Extensions;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel.GetReceipts;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;
        private readonly ILogger logger;

        public AddReceiptCommandHandler(ILogger log, ICommandProducer receiptProducer, ISpendManagementReadModelClient spendManagementReadModelClient)
        {
            logger = log;
            _receiptProducer = receiptProducer;
            _spendManagementReadModelClient = spendManagementReadModelClient;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.AddSpentInputModel.ToCommand();

            await Task.WhenAll(
                request.AddSpentInputModel.ReceiptItems.Select(x => ValidateIfCategoriesExists(x.CategoryId))
                );

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            logger.Information(
                $"Spent created with successfully.",
                () => new
                {
                    receiptCreateCommand
                });

            return receiptCreateCommand.Receipt.Id;
        }

        private async Task ValidateIfCategoriesExists(Guid categoryId)
        {
            await _spendManagementReadModelClient.GetCategoryAsync(categoryId).HandleExceptions("GetCategory");
        }
    }
}
