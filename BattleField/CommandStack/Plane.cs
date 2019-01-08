using System;
using Models.Enums;
using Xer.DomainDriven;

namespace CommandStack
{
    public class Plane : Entity, IUnit
    {
        public UnitType Type { get; }
        public string SerialNumber { get; }
        public string OwnedByPlayer { get; }
        public int Health { get; private set; }
        public bool IsActive { get; private set; }

        public Plane(Guid entityId, UnitType utype, string player) : base(entityId)
        {
            Type = utype;
            OwnedByPlayer = player;
            SerialNumber = $"{utype.ToString()}-{entityId}";
            IsActive = true;
            switch (utype)
            {
                case UnitType.PlaneFighter:
                    Health = 5;
                    break;
                case UnitType.PlaneBomber:
                    Health = 7;
                    break;
                case UnitType.None:
                    //apply the init health later
                    break;
            }
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
