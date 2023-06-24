using GTS.Gqueue.Repositories.Interfaces;

namespace GTS.Gqueue
{
    public interface IUnitOfWork
    {
        IAwaitingPersonRepository AwaitingPersonRepository { get; }
        IQueueRepository QueueRepository { get; }
    }
}
