using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt
{
    public class DeleteReceiptCommandHandler : IRequestHandler<DeleteReceiptCommand, Unit>
    {
        private readonly ICommandProducer _commandProducer;
        public DeleteReceiptCommandHandler(ICommandProducer commandProducer) => _commandProducer = commandProducer;

        public async Task<Unit> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
        {
            var command = request.ToCommand();
            await _commandProducer.ProduceCommandAsync(command);
            return Unit.Value;
        }
    }
}
