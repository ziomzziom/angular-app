using GTS.Gqueue.Entities;

namespace GTS.Gqueue.Repositories.Interfaces
{
    public interface IAwaitingPersonRepository : IRepository<AwaitingPerson>
    {
        Task<IEnumerable<AwaitingPerson>> GetAllFromQueueAsync(Guid queueId);
        Task<int> GetOrderAsync(Guid queueId);
    }
}
