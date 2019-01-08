using System;
using Xer.DomainDriven;

namespace Models.Events.Errors
{
    public class MoveSubmittedToInactiveMatchEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public MoveSubmittedToInactiveMatchEvent(Guid id)
        {
            AggregateRootId = id;
        }
    }
}
