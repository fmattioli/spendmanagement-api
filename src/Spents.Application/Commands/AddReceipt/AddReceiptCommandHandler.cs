using KafkaFlow;
using MediatR;
using Serilog;
using Spents.Application.Commands.AddReceipt;
using Spents.Core.Domain.Interfaces;
using Spents.Events.v1;
using Spents.Topics;
using Spents.Application.InputModels.Extensions;

namespace Spents.Application.Services
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly IReceiptRepository spentRepository;
        private readonly IMessageProducer<ReceiptEvents> eventProducer;
        private readonly ILogger logger;
        public AddReceiptCommandHandler(IReceiptRepository spentRepository, IMessageProducer<ReceiptEvents> eventProducer, ILogger log)
        {
            this.spentRepository = spentRepository;
            this.eventProducer = eventProducer;
            this.logger = log;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var spentEntity = request.AddSpentInputModel.ToEntity();
            var receiptId =  await spentRepository.AddReceipt(spentEntity);
            
            var eventReceipt = spentEntity.ToCreatedEvent();
            await eventProducer.ProduceAsync(KafkaTopics.Events.Receipt, eventReceipt.MessageKey, eventReceipt);

            this.logger.Information(
                   $"Kafka message processed to topic {KafkaTopics.Events.Receipt}.",
                   () => new
                   {
                       eventReceipt
                   });

            return receiptId;
        }
    }
}
