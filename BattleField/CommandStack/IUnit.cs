using System;
using System.Collections.Generic;
using System.Text;
using Models.Enums;

namespace CommandStack
{
    public interface IUnit
    {
        void Heal(int amount);
        void Damage(int amount);

        int Health { get; }
        string OwnedByPlayer { get; }
        UnitType Type { get; }
    }
}
