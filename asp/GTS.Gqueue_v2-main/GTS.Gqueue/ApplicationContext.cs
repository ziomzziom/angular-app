using GTS.Gqueue.Entities;
using Microsoft.EntityFrameworkCore;

namespace GTS.Gqueue
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Queue> Queues { get; set; }
        public DbSet<AwaitingPerson> AwaitingPeople { get; set; }
    }
}
