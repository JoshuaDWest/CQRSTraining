using System;
using Models.Enums;
using Xer.DomainDriven;

namespace Models.Events
{
    public class HealthAddedEvent : IDomainEvent
    {
        public Guid AggregateRootId { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public int Amount { get; }
        public Guid UnitId { get; }
        public UnitType Type { get; }

        public HealthAddedEvent(Guid id, Guid unitId, UnitType type, int amount)
        {
            Amount = amount;
            AggregateRootId = id;
            UnitId = unitId;
            Type = type;
        }
    }
}
