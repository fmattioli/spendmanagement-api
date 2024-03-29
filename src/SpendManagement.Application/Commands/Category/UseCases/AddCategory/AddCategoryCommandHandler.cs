﻿using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Category.UseCases.AddCategory
{
    public class AddCategoryCommandHandler(ICommandProducer receiptProducer) : IRequestHandler<AddCategoryRequestCommand, Guid>
    {
        private readonly ICommandProducer _categoryProducer = receiptProducer;

        public async Task<Guid> Handle(AddCategoryRequestCommand request, CancellationToken cancellationToken)
        {
            var categoryCommand = request
                .AddCategoryInputModel
                .ToCreateCategoryCommand();

            await _categoryProducer.ProduceCommandAsync(categoryCommand);

            return categoryCommand.Category.Id;
        }
    }
}
