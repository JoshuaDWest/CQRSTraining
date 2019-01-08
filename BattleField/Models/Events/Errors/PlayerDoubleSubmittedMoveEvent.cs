using System;
using System.Collections.Generic;
using System.Text;
using Xer.DomainDriven;

namespace Models.Events.Errors
{
    public class PlayerDoubleSubmittedMoveEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }

        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public string Player { get; }
        public PlayerDoubleSubmittedMoveEvent(Guid aggregateId, string player)
        {
            Player = player;
            AggregateRootId = aggregateId;
        }
    }
}
