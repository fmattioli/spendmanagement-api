﻿using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.UseCases.DeleteReceipt
{
    public class DeleteReceiptCommandHandler(ICommandProducer commandProducer) : IRequest
    {
        private readonly ICommandProducer _commandProducer = commandProducer;

        public async Task Handle(DeleteReceiptCommand request)
        {
            var command = request.ToCommand();
            await _commandProducer.ProduceCommandAsync(command);
        }
    }
}
