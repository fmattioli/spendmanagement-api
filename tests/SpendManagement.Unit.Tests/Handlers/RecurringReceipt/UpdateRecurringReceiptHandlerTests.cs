using AutoFixture;
using Moq;
using SpendManagement.Application.Commands.RecurringReceipt.UseCases.UpdateRecurringReceipt;
using SpendManagement.Application.Producers;

namespace SpendManagement.Unit.Tests.Handlers.RecurringReceipt
{
    public class UpdateRecurringReceiptHandlerTests
    {
        private readonly UpdateRecurringReceiptCommandHandler handler;
        private readonly Mock<ICommandProducer> commandProducerMock = new();
        private readonly Fixture fixture = new();

        public UpdateRecurringReceiptHandlerTests()
        {
                
        }
    }
}
