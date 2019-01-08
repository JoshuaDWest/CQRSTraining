using System;
using Xer.DomainDriven;

namespace Models.Events
{
    public class AllPlayerMovesSubmittedForTurnEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }

        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public string Player { get; }
        public int Turn { get; }

        public AllPlayerMovesSubmittedForTurnEvent(Guid id, string player, int turn)
        {
            AggregateRootId = id;
            Player = player;
            Turn = turn;
        }
    }
}
