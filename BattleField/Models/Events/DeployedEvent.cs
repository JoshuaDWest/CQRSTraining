using System;
using Models.Enums;
using Xer.DomainDriven;

namespace Models.Events
{
    public class DeployedEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public string Player { get; }
        public UnitType Type { get; }
        public Guid DeployedId { get; }

        public DeployedEvent(Guid id, Guid deployed, string player, UnitType type)
        {
            AggregateRootId = id;
            DeployedId = deployed;
            Player = player;
            Type = type;
        }
    }
}
