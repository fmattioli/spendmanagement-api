using MediatR;
using Serilog;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer;
        private readonly ILogger logger;

        public AddReceiptCommandHandler(ILogger log, ICommandProducer receiptProducer)
        {
            logger = log;
            _receiptProducer = receiptProducer;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.AddSpentInputModel.ToCommand();

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            logger.Information(
                $"Spent created with successfully.",
                () => new
                {
                    receiptCreateCommand
                });

            return receiptCreateCommand.Receipt.Id;
        }
    }
}
