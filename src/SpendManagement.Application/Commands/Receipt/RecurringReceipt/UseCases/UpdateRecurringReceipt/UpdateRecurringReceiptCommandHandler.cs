using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Application.Services;
using SpendManagement.Client.Extensions;
using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Commands.Receipt.RecurringReceipt.UseCases.UpdateRecurringReceipt
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
                .GetRecurringReceiptAsync(request.Id);

            var recurringReceipt = receiptPagedResult.Results.FirstOrDefault();

            if (recurringReceipt != null)
            {
                request
                    .UpdateRecurringReceiptInputModel
                    .RecurringReceiptPatchDocument
                    .ApplyTo(recurringReceipt, JsonPatchExtension.HandlePatchErrors(_validator));

                await _receiptService.ValidateIfCategoryExistAsync(recurringReceipt.CategoryId);

                await _receiptProducer.ProduceCommandAsync(recurringReceipt.ToCommand());
            }
        }
    }
}
