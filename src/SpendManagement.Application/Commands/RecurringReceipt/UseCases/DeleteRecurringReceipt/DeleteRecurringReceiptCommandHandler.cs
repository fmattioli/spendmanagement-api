using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.RecurringReceipt.UseCases.DeleteRecurringReceipt
{
    public class DeleteRecurringReceiptCommandHandler(ICommandProducer commandProducer) : IRequest
    {
        private readonly ICommandProducer _commandProducer = commandProducer;

        public async Task Handle(DeleteRecurringReceiptCommand request)
        {
            var command = request.ToCommand();
            await _commandProducer.ProduceCommandAsync(command);
        }
    }
}
