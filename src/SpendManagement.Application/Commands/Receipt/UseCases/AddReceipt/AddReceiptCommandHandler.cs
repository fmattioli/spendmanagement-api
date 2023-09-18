using MediatR;
using Microsoft.AspNetCore.Http;

using Serilog;
using SpendManagement.Application.Commands.Receipt.Services;
using SpendManagement.Application.Mappers;
using SpendManagement.Application.Producers;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly ICommandProducer _receiptProducer;
        private readonly IReceiptService _receiptService;
        private readonly ILogger logger;

        public AddReceiptCommandHandler(ILogger log, ICommandProducer receiptProducer, IReceiptService receiptService)
        {
            logger = log;
            _receiptProducer = receiptProducer;
            _receiptService = receiptService;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var receiptCreateCommand = request.AddSpentInputModel.ToCommand();

            await Task.WhenAll(
                request.AddSpentInputModel.ReceiptItems.Select(x => _receiptService.ValidateIfCategoriesExists(x.CategoryId))
                );

            await _receiptProducer.ProduceCommandAsync(receiptCreateCommand);

            logger.Information(
                "Spent created with successfully.",
                () => new
                {
                    receiptCreateCommand
                });

            return receiptCreateCommand.Receipt.Id;
        }
    }
}
