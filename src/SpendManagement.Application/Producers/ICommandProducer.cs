using SpendManagement.Contracts.V1.Interfaces;

namespace SpendManagement.Application.Producers
{
    public interface ICommandProducer
    {
        Task ProduceCommandAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
