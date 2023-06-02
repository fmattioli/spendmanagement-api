using FluentValidation.Results;
using KafkaFlow;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using SpendManagement.Application.Commands.UpdateReceipt.Exceptions;
using SpendManagement.Client.SpendManagementReadModel.GetReceipts;
using SpendManagement.Contracts.V1.Commands.Interfaces;

namespace SpendManagement.Application.Commands.UpdateReceipt
{
    public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand, Unit>
    {
        private readonly IMessageProducer<ICommand> _commandsProducer;
        private readonly ILogger _logger;
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;

        public UpdateReceiptCommandHandler(ILogger log,
            IMessageProducer<ICommand> commandsProducer,
            ISpendManagementReadModelClient spendManagementReadModelClient)
        {
            this._logger = log;
            this._commandsProducer = commandsProducer;
            this._spendManagementReadModelClient = spendManagementReadModelClient;
        }

        public async Task<Unit> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            var receipt = await _spendManagementReadModelClient.GetReceiptAsync(request.UpdateReceiptInputModel.Id) ?? throw new NotFoundException("Any recept was found");

            var validationResult = new ValidationResult();

            request.UpdateReceiptInputModel.ReceiptPatchDocument.ApplyTo(receipt, HandlePatchErrors(validationResult));
            if (validationResult.IsValid)
            {
                throw new JsonPatchInvalidException("Any recept was found");
            }
            
            return Unit.Value;
        }

        private static Action<JsonPatchError> HandlePatchErrors(ValidationResult validationResult)
        {
            return error => validationResult.Errors.Add(
                new ValidationFailure(error.Operation.path, error.ErrorMessage)
                {
                    ErrorCode = "104"
                });
        }
    }
}
