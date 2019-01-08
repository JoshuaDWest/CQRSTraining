using System;
using Models.Enums;
using Xer.DomainDriven;

namespace Models.Events
{
    public class DestroyedEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public string Player { get; }
        public UnitType Type { get; }
        public Guid UnitId { get; set; }
        public DestroyedEvent(Guid id, Guid unitId, string player, UnitType type)
        {
            UnitId = unitId;
            AggregateRootId = id;
            Player = player;
            Type = type;
        }
    }
}
