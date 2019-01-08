using System;
using System.Collections.Generic;
using System.Text;
using Xer.DomainDriven;

namespace Models.Events
{
    public class MatchStartedEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public MatchStartedEvent(Guid id)
        {
            AggregateRootId = id;
        }
    }
}
