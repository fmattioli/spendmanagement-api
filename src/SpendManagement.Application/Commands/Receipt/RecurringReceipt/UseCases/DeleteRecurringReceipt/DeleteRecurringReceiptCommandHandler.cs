using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.RecurringReceipt.UseCases.DeleteRecurringReceipt
{
    public class DeleteRecurringReceiptCommandHandler(ICommandProducer commandProducer) : IRequestHandler<DeleteRecurringReceiptCommand>
    {
        private readonly ICommandProducer _commandProducer = commandProducer;

        public async Task Handle(DeleteRecurringReceiptCommand request, CancellationToken cancellationToken)
        {
            var command = request.ToCommand();
            await _commandProducer.ProduceCommandAsync(command);
        }
    }
}
