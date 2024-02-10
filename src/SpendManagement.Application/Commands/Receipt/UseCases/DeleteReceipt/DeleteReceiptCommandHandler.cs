using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt
{
    public class DeleteReceiptCommandHandler(ICommandProducer commandProducer) : IRequestHandler<DeleteReceiptCommand>
    {
        private readonly ICommandProducer _commandProducer = commandProducer;

        public async Task Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
        {
            var command = request.ToCommand();
            await _commandProducer.ProduceCommandAsync(command);
        }
    }
}
