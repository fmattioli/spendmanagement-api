using KafkaFlow;
using Serilog;
using SpendManagement.Contracts.V1.Interfaces;

namespace SpendManagement.Application.Producers
{
    public class CommandProducer : ICommandProducer
    {
        private readonly IMessageProducer<ICommand> _messageProducer;
        private readonly ILogger logger;

        public CommandProducer(IMessageProducer<ICommand> messageProducer, ILogger log) => (_messageProducer, logger) = (messageProducer, log);

        public async Task ProduceCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _messageProducer.ProduceAsync(command.RoutingKey, command);

            logger.Information("Command produced with success. Command details: {@Command}", command);
        }
    }
}
