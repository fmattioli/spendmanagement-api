using KafkaFlow;

using MediatR;
using Spents.Application.Commands.AddSpent;
using Spents.Core.Interfaces;
using Spents.Events.Events.v1;
using Spents.Topics;

namespace Spents.Application.Services
{
    public class AddSpentsCommandHandler : IRequestHandler<AddSpentCommand, Guid>
    {
        private readonly IReceiptRepository spentRepository;
        private readonly IMessageProducer<ReceiptCreatedEvent> eventProducer;
        public AddSpentsCommandHandler(IReceiptRepository spentRepository, IMessageProducer<ReceiptCreatedEvent> eventProducer)
        {
            this.spentRepository = spentRepository;
            this.eventProducer = eventProducer;

        }
        public async Task<Guid> Handle(AddSpentCommand request, CancellationToken cancellationToken)
        {
            var spent = request.AddSpentInputModel.ToEntity();
            var receiptId =  await spentRepository.AddReceipt(spent);
            await eventProducer.ProduceAsync(KafkaTopics.Events.Receipt, receiptId, request.AddSpentInputModel.ToEvent());
            return receiptId;

        }
    }
}
