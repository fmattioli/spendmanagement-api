using Spents.Core.Entities;

namespace Spents.Core.Interfaces
{
    public interface ISpentRepository
    {
        Task<Guid> AddSpent(Receipt receipt);
    }
}
