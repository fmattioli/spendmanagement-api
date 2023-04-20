using KafkaFlow;
using MediatR;
using Serilog;
using Spents.Application.Commands.AddReceipt;
using Spents.Core.Domain.Interfaces;
using Spents.Events.v1;
using Spents.Topics;
using Spents.Application.InputModels.Extensions;
using Spents.Core.Domain.Entities;

namespace Spents.Application.Services
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly IReceiptRepository spentRepository;
        private readonly IMessageProducer<ReceiptEvent<ReceiptEntity>> eventProducer;
        private readonly ILogger logger;
        public AddReceiptCommandHandler(IReceiptRepository spentRepository, IMessageProducer<ReceiptEvent<ReceiptEntity>> eventProducer, ILogger log)
        {
            this.spentRepository = spentRepository;
            this.eventProducer = eventProducer;
            this.logger = log;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var spentEntity = request.AddSpentInputModel.ToEntity();
            var receiptId =  await spentRepository.AddReceipt(spentEntity);
            
            var eventCreatedReceipt = spentEntity.ToReceiptCreatedEvent();
            var receiptDocument = spentEntity.ToReceiptDocument();

            await eventProducer.ProduceAsync(KafkaTopics.Events.ReceiptEvents, receiptId.ToString(), eventCreatedReceipt);
            await eventProducer.ProduceAsync(KafkaTopics.Documents.ReceiptDocuments, receiptId.ToString(), receiptDocument);

            this.logger.Information(
                   $"Spent created with succesfully.",
                   () => new
                   {
                       eventCreatedReceipt
                   });

            return receiptId;
        }
    }
}
