using System;
using Xer.DomainDriven;

namespace Models.Events
{
    public class MatchTurnEndedEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public int Turn { get; }

        public MatchTurnEndedEvent(Guid id, int turn)
        {
            AggregateRootId = id;
            Turn = turn;
        }
    }
}
