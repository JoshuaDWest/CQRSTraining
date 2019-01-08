using System;
using Models.Enums;
using Xer.DomainDriven;

namespace CommandStack
{
    public class Tank : Entity, IUnit
    {
        public UnitType Type { get; }
        public int Health { get; private set; }
        public string SerialNumber { get; }
        public bool IsActive { get; private set; }
        public string OwnedByPlayer { get; }

        public Tank(Guid entityId, UnitType utype, string player) : base(entityId)
        {
            OwnedByPlayer = player;
            Type = utype;
            IsActive = true;
            SerialNumber = $"{utype.ToString()}-{entityId}";
            switch (utype)
            {
                case UnitType.TankLight:
                    Health = 8;
                    break;
                case UnitType.TankHeavy:
                    Health = 10;
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
