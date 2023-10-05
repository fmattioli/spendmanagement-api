using FluentValidation.Results;
using MediatR;
using Serilog;
using SpendManagement.Application.Commands.Receipt.Services;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using SpendManagement.Application.Extensions;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Commands.Receipt.UpdateReceipt
{
    public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand, Unit>
    {
        private readonly ICommandProducer _receiptProducer;
        private readonly IReceiptService _receiptService;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;
        private readonly ILogger _logger;

        public UpdateReceiptCommandHandler(ICommandProducer receiptProducer,
            ISpendManagementReadModelClient spendManagementReadModelClient,
            IReceiptService receiptService,
            ILogger logger)
        {
            _receiptProducer = receiptProducer;
            _spendManagementReadModelClient = spendManagementReadModelClient;
            _receiptService = receiptService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            var receipt = await _spendManagementReadModelClient.GetReceiptAsync(request.UpdateReceiptInputModel.Id) ?? throw new NotFoundException("Any recept was found");

            var validationResult = new ValidationResult();

            request.UpdateReceiptInputModel.ReceiptPatchDocument.ApplyTo(receipt, JsonPatchExtension.HandlePatchErrors(validationResult));
            if (!validationResult.IsValid)
            {
                _logger.Error("Invalid json provided.: {@Errors}", validationResult.Errors);

                throw new JsonPatchInvalidException(string.Join(",", validationResult.Errors));
            }

            await Task.WhenAll(
                receipt.ReceiptItems.Select(x => _receiptService.ValidateIfCategoriesExists(x.CategoryId)));

            await _receiptProducer.ProduceCommandAsync(receipt.ToCommand());

            return Unit.Value;
        }
    }
}
