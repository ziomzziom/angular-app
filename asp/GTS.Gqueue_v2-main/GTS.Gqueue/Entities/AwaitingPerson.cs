namespace GTS.Gqueue.Entities
{
    public class AwaitingPerson
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public Guid QueueId { get; set; }
        public virtual Queue? Queue { get; set; }
        public bool Dequeued { get; set; }
    }
}
