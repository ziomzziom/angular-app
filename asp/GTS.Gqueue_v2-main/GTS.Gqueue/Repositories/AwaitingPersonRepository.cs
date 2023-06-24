using GTS.Gqueue.Entities;
using GTS.Gqueue.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GTS.Gqueue.Repositories
{
    public class AwaitingPersonRepository : Repository<AwaitingPerson>, IAwaitingPersonRepository
    {
        public AwaitingPersonRepository(ApplicationContext context) : base(context)
        {
        }

        public new async Task<AwaitingPerson> GetByIdAsync(Guid id)
        {
            return await _context.AwaitingPeople.Include(x => x.Queue).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<AwaitingPerson>> GetAllFromQueueAsync(Guid queueId)
        {
            return await _context.AwaitingPeople.Include(x => x.Queue).Where(x => x.QueueId == queueId).ToListAsync();
        }

        public async Task<int> GetOrderAsync(Guid queueId)
        {
            return await _context.AwaitingPeople.Where(x => x.QueueId == queueId).OrderByDescending(x => x.Order).Select(x => x.Order).FirstOrDefaultAsync() + 1;

        }
    }
}
