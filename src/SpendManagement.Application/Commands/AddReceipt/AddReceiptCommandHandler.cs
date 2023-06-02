using KafkaFlow;
using MediatR;
using Serilog;
using SpendManagement.Application.Commands.AddReceipt;
using SpendManagement.Topics;
using SpendManagement.Application.InputModels.Extensions;
using SpendManagement.Contracts.V1.Commands.Interfaces;

namespace SpendManagement.Application.Services
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Unit>
    {
        private readonly IMessageProducer<ICommand> commandsProducer;
        private readonly ILogger logger;

        public AddReceiptCommandHandler(ILogger log,
            IMessageProducer<ICommand> commandsProducer)
        {
            this.logger = log;
            this.commandsProducer = commandsProducer;
        }

        public async Task<Unit> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.AddSpentInputModel.ToCommand();

            await commandsProducer.ProduceAsync(KafkaTopics.Commands.ReceiptCommandTopicName, receiptCreateCommand.RoutingKey, receiptCreateCommand);

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
