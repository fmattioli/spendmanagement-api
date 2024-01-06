using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;
using SpendManagement.Client.Extensions;
using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Commands.Receipt.UpdateReceipt
{
    public class UpdateReceiptCommandHandler(ICommandProducer receiptProducer,
        ISpendManagementReadModelClient spendManagementReadModelClient,
        IReceiptService receiptService,
        IValidator<JsonPatchError> validator) : IRequestHandler<UpdateReceiptCommand, Unit>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        private readonly IReceiptService _receiptService = receiptService;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient = spendManagementReadModelClient;
        private readonly IValidator<JsonPatchError> _validator = validator;

        public async Task<Unit> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptPagedResult = await _spendManagementReadModelClient
                .GetReceiptAsync(request.UpdateReceiptInputModel.Id);

            var receipt = receiptPagedResult.Results.FirstOrDefault();

            if (receipt != null)
            {
                request
                    .UpdateReceiptInputModel
                    .ReceiptPatchDocument
                    .ApplyTo(receipt, JsonPatchExtension.HandlePatchErrors(_validator));

                await Task.WhenAll(
                    receipt.ReceiptItems.Select(x => _receiptService.ValidateIfCategoryExistAsync(x.CategoryId)));

                await _receiptProducer.ProduceCommandAsync(receipt.ToCommand());
            }

            return Unit.Value;
        }
    }
}
