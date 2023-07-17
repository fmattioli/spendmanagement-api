using MediatR;
using Serilog;
using SpendManagement.Application.Commands.AddReceipt;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Services
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Unit>
    {
        private readonly ICommandProducer _receiptProducer;
        private readonly ILogger logger;

        public AddReceiptCommandHandler(ILogger log, ICommandProducer receiptProducer)
        {
            this.logger = log;
            this._receiptProducer = receiptProducer;
        }

        public async Task<Unit> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.AddSpentInputModel.ToCommand();

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            this.logger.Information(
                $"Spent created with successfully.",
                () => new
                {
                    receiptCreateCommand
                });

            return Unit.Value;
        }
    }
}
