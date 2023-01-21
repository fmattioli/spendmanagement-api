using KafkaFlow;

using MediatR;

using Spents.Application.Commands.AddReceipt;
using Spents.Core.Interfaces;
using Spents.Events.Events.v1;
using Spents.Topics;

namespace Spents.Application.Services
{
    public class AddReceiptCommandHandler : IRequestHandler<AddReceiptCommand, Guid>
    {
        private readonly IReceiptRepository spentRepository;
        private readonly IMessageProducer<ReceiptCreatedEvent> eventProducer;
        public AddReceiptCommandHandler(IReceiptRepository spentRepository, IMessageProducer<ReceiptCreatedEvent> eventProducer)
        {
            this.spentRepository = spentRepository;
            this.eventProducer = eventProducer;
        }

        public async Task<Guid> Handle(AddReceiptCommand request, CancellationToken cancellationToken)
        {
            var spent = request.AddSpentInputModel.ToEntity();
            var receiptId =  await spentRepository.AddReceipt(spent);
            var eventReceipt = request.AddSpentInputModel.ToEvent(receiptId);
            await eventProducer.ProduceAsync(KafkaTopics.Events.Receipt, eventReceipt.MessageKey, eventReceipt);
            return receiptId;

        }
    }
}
