using GTS.Gqueue.Entities;
using GTS.Gqueue.Repositories.Interfaces;

namespace GTS.Gqueue.Repositories
{
    public class QueueRepository : Repository<Queue>, IQueueRepository
    {
        public QueueRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
