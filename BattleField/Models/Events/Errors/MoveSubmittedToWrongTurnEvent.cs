using System;
using Xer.DomainDriven;

namespace Models.Events.Errors
{
    public class MoveSubmittedToWrongTurnEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public MoveSubmittedToWrongTurnEvent(Guid id)
        {
            AggregateRootId = id;
        }
    }
}
