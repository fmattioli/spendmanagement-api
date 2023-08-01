using FluentValidation.Results;
using MediatR;
using SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions;
using SpendManagement.Application.Extensions;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;
using SpendManagement.Client.SpendManagementReadModel.GetReceipts;

namespace SpendManagement.Application.Commands.Receipt.UpdateReceipt
{
    public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand, Unit>
    {
        private readonly ICommandProducer _receiptProducer;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;

        public UpdateReceiptCommandHandler(ICommandProducer receiptProducer,
            ISpendManagementReadModelClient spendManagementReadModelClient)
        {
            _receiptProducer = receiptProducer;
            _spendManagementReadModelClient = spendManagementReadModelClient;
        }

        public async Task<Unit> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            var receipt = await _spendManagementReadModelClient.GetReceiptAsync(request.UpdateReceiptInputModel.Id) ?? throw new NotFoundException("Any recept was found");

            var validationResult = new ValidationResult();

            request.UpdateReceiptInputModel.ReceiptPatchDocument.ApplyTo(receipt, JsonPatchExtension.HandlePatchErrors(validationResult));
            if (!validationResult.IsValid)
            {
                throw new JsonPatchInvalidException(string.Join(",", validationResult.Errors));
            }

            await _receiptProducer.ProduceCommandAsync(receipt.ToCommand());
            return Unit.Value;
        }
    }
}
