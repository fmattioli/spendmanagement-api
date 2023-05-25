using KafkaFlow;
using MediatR;
using Serilog;
using SpendManagement.Contracts.V1.Commands.Interfaces;

namespace SpendManagement.Application.Commands.UpdateReceipt
{
    public class UpdateReceiptCommandItemHandler : IRequest
    {
        private readonly IMessageProducer<ICommand> commandsProducer;
        private readonly ILogger logger;

        public UpdateReceiptCommandItemHandler(ILogger log,
            IMessageProducer<ICommand> commandsProducer)
        {
            this.logger = log;
            this.commandsProducer = commandsProducer;
        }

        public async Task Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
