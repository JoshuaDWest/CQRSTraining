using System;
using Models.Enums;
using Models.Events;
using Xer.DomainDriven;

namespace CommandStack
{
    public class Soldier : Entity, IUnit
    {
        public string Name { get; }
        public int Health { get; private set; }
        public bool IsActive { get; private set; }
        public string OwnedByPlayer { get; }
        public UnitType Type => UnitType.Infantry;
        public Soldier(Guid entityId, string name, string player) : base(entityId)
        {
            OwnedByPlayer = player;
            Name = name;
            Health = 1;
            IsActive = true;
        }

        public void Damage(int amount)
        {
            Health -= amount;
            if (Health <= 0) IsActive = false;
        }

        public void Heal(int amount)
        {
            Health += amount;
            if (Health > 0) IsActive = true;
        }
    }
}
