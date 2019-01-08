using System;
using Xer.DomainDriven;

namespace Models.Events
{
    public class MatchEndedEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public string Winner { get; }
        public MatchEndedEvent(Guid id, string winner)
        {
            AggregateRootId = id;
            Winner = winner;
        }
    }
}
