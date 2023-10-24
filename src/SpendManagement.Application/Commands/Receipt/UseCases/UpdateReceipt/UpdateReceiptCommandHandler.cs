using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
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
        private readonly IValidator<JsonPatchError> _validator;

        public UpdateReceiptCommandHandler(ICommandProducer receiptProducer,
            ISpendManagementReadModelClient spendManagementReadModelClient,
            IReceiptService receiptService,
            IValidator<JsonPatchError> validator)
        {
            _receiptProducer = receiptProducer;
            _spendManagementReadModelClient = spendManagementReadModelClient;
            _receiptService = receiptService;
            _validator = validator;
        }

        public async Task<Unit> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            var receipt = await _spendManagementReadModelClient.GetReceiptAsync(request.UpdateReceiptInputModel.Id) ?? throw new NotFoundException("Any recept was found");

            request
                .UpdateReceiptInputModel
            .ReceiptPatchDocument
                .ApplyTo(receipt, JsonPatchExtension.HandlePatchErrors(_validator));

            await Task.WhenAll(
                receipt.ReceiptItems.Select(x => _receiptService.ValidateIfCategoriesExists(x.CategoryId)));

            await _receiptProducer.ProduceCommandAsync(receipt.ToCommand());

            return Unit.Value;
        }
    }
}
