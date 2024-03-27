using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.VariableReceipt.UseCases.DeleteReceipt
{
    public class DeleteVariableReceiptCommandHandler(ICommandProducer commandProducer) : IRequestHandler<DeleteVariableReceiptCommand>
    {
        private readonly ICommandProducer _commandProducer = commandProducer;

        public async Task Handle(DeleteVariableReceiptCommand request, CancellationToken cancellationToken)
        {
            var command = request.ToCommand();
            await _commandProducer.ProduceCommandAsync(command);
        }
    }
}
