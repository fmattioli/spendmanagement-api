using MediatR;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Category.UseCases.AddCategory
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Unit>
    {
        private readonly ICommandProducer _categoryProducer;

        public AddCategoryCommandHandler(ICommandProducer receiptProducer)
        {
            _categoryProducer = receiptProducer;
        }

        public async Task<Unit> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryCommand = request.AddCategoryInputModel.ToCreateCategoryCommand();
            await _categoryProducer.ProduceCommandAsync(categoryCommand);
            return Unit.Value;
        }
    }
}
