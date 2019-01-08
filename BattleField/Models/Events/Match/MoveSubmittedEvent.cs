using System;
using Models.Enums;
using Xer.DomainDriven;

namespace Models.Events
{
    public class MoveSubmittedEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public int Turn { get; }
        public string Player { get; }
        /// <summary>
        /// was this a deployment or an attack?
        /// </summary>
        public bool Deployment { get; }
        public UnitType Source { get; }
        public UnitType Target { get; }

        /// <summary>
        /// Represents a player attacking another player
        /// </summary>
        /// <param name="id"></param>
        /// <param name="turn"></param>
        /// <param name="player"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public MoveSubmittedEvent(Guid id, int turn, string player, UnitType source, UnitType target)
        {
            AggregateRootId = id;
            Turn = turn;
            Player = player;
            Source = source;
            Target = target;
            Deployment = false;
        }

        /// <summary>
        /// Represents a player deploying a unit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="turn"></param>
        /// <param name="player"></param>
        /// <param name="deployed"></param>
        public MoveSubmittedEvent(Guid id, int turn, string player, UnitType deployed)
        {
            AggregateRootId = id;
            Turn = turn;
            Player = player;
            Source = deployed;
            Target = UnitType.None;
            Deployment = true;
        }
    }
}
