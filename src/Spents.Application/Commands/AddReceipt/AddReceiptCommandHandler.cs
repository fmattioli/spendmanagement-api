using KafkaFlow;
using MediatR;
using Serilog;
using Spents.Application.Commands.AddReceipt;
using Spents.Core.Domain.Interfaces;
using Spents.Events.v1;
using Spents.Topics;
using Spents.Application.InputModels.Extensions;
using Spents.Core.Domain.Entities;
using Spents.Contracts.Documents;

namespace Spents.Application.Services
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly IReceiptRepository spentRepository;
        private readonly IMessageProducer<ReceiptEvent<ReceiptEntity>> eventProducer;
        private readonly IMessageProducer<ReceiptDocument> commandsProducer;
        private readonly ILogger logger;

        public AddReceiptCommandHandler(IReceiptRepository spentRepository, 
            IMessageProducer<ReceiptEvent<ReceiptEntity>> eventProducer, 
            ILogger log,
            IMessageProducer<ReceiptDocument> commandsProducer)
        {
            this.spentRepository = spentRepository;
            this.eventProducer = eventProducer;
            this.logger = log;
            this.commandsProducer = commandsProducer;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var spentEntity = request.AddSpentInputModel.ToEntity();
            var receiptId =  await spentRepository.AddReceipt(spentEntity);
            
            var eventCreatedReceipt = spentEntity.ToReceiptCreatedEvent();
            var receiptDocument = spentEntity.ToReceiptDocument();

            await commandsProducer.ProduceAsync(KafkaTopics.Documents.ReceiptDocuments, receiptId.ToString(), receiptDocument);
            await eventProducer.ProduceAsync(KafkaTopics.Events.ReceiptEvents, receiptId.ToString(), eventCreatedReceipt);

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
