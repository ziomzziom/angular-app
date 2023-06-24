using Microsoft.EntityFrameworkCore;

namespace GTS.Gqueue.Entities
{
    public class Queue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
