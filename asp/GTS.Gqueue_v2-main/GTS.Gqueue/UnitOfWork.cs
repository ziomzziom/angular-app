using GTS.Gqueue.Repositories.Interfaces;

namespace GTS.Gqueue
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IQueueRepository queueRepository, IAwaitingPersonRepository awaitingPersonRepository)
        {
            QueueRepository = queueRepository;
            AwaitingPersonRepository = awaitingPersonRepository;
        }

        public IQueueRepository QueueRepository { get; }
        public IAwaitingPersonRepository AwaitingPersonRepository { get; }
    }
}
