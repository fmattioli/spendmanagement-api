﻿using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Category.UseCases.DeleteCategory
{
    public class DeleteCategoryCommandHandler(ICommandProducer receiptProducer) : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICommandProducer _categoryProducer = receiptProducer;

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryCommand = request.ToCommand();
            await _categoryProducer.ProduceCommandAsync(categoryCommand);
            return Unit.Value;
        }
    }
}
