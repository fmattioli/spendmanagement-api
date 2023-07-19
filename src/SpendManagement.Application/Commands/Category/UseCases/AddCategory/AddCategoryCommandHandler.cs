﻿using MediatR;
using Serilog;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Category.UseCases.AddCategory
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Unit>
    {
        private readonly ICommandProducer _categoryProducer;
        private readonly ILogger logger;

        public AddCategoryCommandHandler(ILogger log, ICommandProducer receiptProducer)
        {
            logger = log;
            _categoryProducer = receiptProducer;
        }

        public async Task<Unit> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryCommand = request.AddCategoryInputModel.ToCommand();
            await _categoryProducer.ProduceCommandAsync(categoryCommand);
            return Unit.Value;
        }
    }
}