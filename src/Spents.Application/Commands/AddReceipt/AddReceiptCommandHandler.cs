using KafkaFlow;
using MediatR;
using Serilog;
using Spents.Application.Commands.AddReceipt;
using Spents.Topics;
using Spents.Application.InputModels.Extensions;
using Spents.Contracts.V1.Commands.Interfaces;
using Spents.Contracts.V1.Commands;

namespace Spents.Application.Services
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly IMessageProducer<ICommand> commandsProducer;
        private readonly ILogger logger;

        public AddReceiptCommandHandler(ILogger log,
            IMessageProducer<ICommand> commandsProducer)
        {
            this.logger = log;
            this.commandsProducer = commandsProducer;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.AddSpentInputModel.ToCommand();

            await commandsProducer.ProduceAsync(KafkaTopics.Commands.ReceitCommandTopicName, receiptCreateCommand.RoutingKey, receiptCreateCommand);

            this.logger.Information(
                $"Spent created with succesfully.",
                () => new
                {
                    receiptCreateCommand
                });

            return receiptCreateCommand.Id;
        }
    }
}
