using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;
using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Commands.RecurringReceipt.UseCases.UpdateRecurringReceipt
{
    public class UpdateRecurringReceiptCommandHandler(ICommandProducer receiptProducer,
        ISpendManagementReadModelClient spendManagementReadModelClient,
        IReceiptService receiptService,
        IValidator<JsonPatchError> validator) : IRequestHandler<UpdateRecurringReceiptCommand>
    {
        private readonly ICommandProducer _receiptProducer = receiptProducer;
        private readonly IReceiptService _receiptService = receiptService;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient = spendManagementReadModelClient;
        private readonly IValidator<JsonPatchError> _validator = validator;

        public async Task Handle(UpdateRecurringReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptPagedResult = await _spendManagementReadModelClient
                .GetReceiptAsync(request.Id);

            var receipt = receiptPagedResult.Results.FirstOrDefault();

            if (receipt != null)
            {
                //request
                //    .UpdateReceiptInputModel
                //    .ReceiptPatchDocument
                //    .ApplyTo(receipt, JsonPatchExtension.HandlePatchErrors(_validator));

                await _receiptService.ValidateIfCategoryExistAsync(receipt.CategoryId);

                await _receiptProducer.ProduceCommandAsync(receipt.ToCommand());
            }
        }
    }
}
