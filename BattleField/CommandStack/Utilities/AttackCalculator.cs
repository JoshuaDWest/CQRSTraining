using Models.Enums;
using Models.Exceptions;
using System;

namespace CommandStack.Utilities
{
    // This could be simplified greatly by using matrices or the like, but this is at least explicit i guess

    /// <summary>
    /// Handles calculating unit attacks
    /// </summary>
    internal static class AttackCalculator
    {
        private const int HeavyTank_Soldier_Chance = 50;
        private const int HeavyTank_FighterPlane_Chance = 0;
        private const int HeavyTank_BomberPlane_Chance = 0;
        private const int HeavyTank_LightTank_Chance = 80;
        private const int HeavyTank_HeavyTank_Chance = 80;
        private const int LightTank_Soldier_Chance = 70;
        private const int LightTank_FighterPlane_Chance = 0;
        private const int LightTank_BomberPlane_Chance = 0;
        private const int LightTank_LightTank_Chance = 80;
        private const int LightTank_HeavyTank_Chance = 80;
        private const int FighterPlane_Soldier_Chance = 0;
        private const int FighterPlane_FighterPlane_Chance = 70;
        private const int FighterPlane_BomberPlane_Chance = 70;
        private const int FighterPlane_LightTank_Chance = 70;
        private const int FighterPlane_HeavyTank_Chance = 70;
        private const int BomberPlane_Soldier_Chance = 0;
        private const int BomberPlane_FighterPlane_Chance = 50;
        private const int BomberPlane_BomberPlane_Chance = 50;
        private const int BomberPlane_LightTank_Chance = 80;
        private const int BomberPlane_HeavyTank_Chance = 80;
        private const int Soldier_Soldier_Chance = 70;
        private const int Soldier_LightTank_Chance = 100;
        private const int Soldier_HeavyTank_Chance = 100;
        private const int Soldier_FighterPlane_Chance = 0;
        private const int Soldier_BomberPlane_Chance = 0;

        private const int HeavyTank_Soldier_Dmg = 1;
        private const int HeavyTank_FighterPlane_Dmg = 0;
        private const int HeavyTank_BomberPlane_Dmg = 0;
        private const int HeavyTank_LightTank_Dmg = 5;
        private const int HeavyTank_HeavyTank_Dmg = 5;
        private const int LightTank_Soldier_Dmg = 1;
        private const int LightTank_FighterPlane_Dmg = 0;
        private const int LightTank_BomberPlane_Dmg = 0;
        private const int LightTank_LightTank_Dmg = 3;
        private const int LightTank_HeavyTank_Dmg = 3;
        private const int FighterPlane_Soldier_Dmg = 0;
        private const int FighterPlane_FighterPlane_Dmg = 5;
        private const int FighterPlane_BomberPlane_Dmg = 7;
        private const int FighterPlane_LightTank_Dmg = 3;
        private const int FighterPlane_HeavyTank_Dmg = 3;
        private const int BomberPlane_Soldier_Dmg = 0;
        private const int BomberPlane_FighterPlane_Dmg = 5;
        private const int BomberPlane_BomberPlane_Dmg = 7;
        private const int BomberPlane_LightTank_Dmg = 5;
        private const int BomberPlane_HeavyTank_Dmg = 5;
        private const int Soldier_Soldier_Dmg = 1;
        private const int Soldier_LightTank_Dmg = 2;
        private const int Soldier_HeavyTank_Dmg = 2;
        private const int Soldier_FighterPlane_Dmg = 0;
        private const int Soldier_BomberPlane_Dmg = 0;



        private static Random _random = new Random((int)DateTime.Now.Ticks);
        public static (bool success, int dmg, Exception exc) Attack(UnitType attacker, UnitType attacked)
        {
            switch (attacker)
            {
                case UnitType.TankHeavy:
                case UnitType.TankLight:
                    return TankAttack(attacker, attacked);
                case UnitType.PlaneBomber:
                case UnitType.PlaneFighter:
                    return PlaneAttack(attacker, attacked);
                case UnitType.Infantry:
                    return SoldierAttack(attacked);
                default:
                    return (false, 0, new CannotCalculateAttackException("Unknown unit types cannot attack"));
            }
            return (false, 0, new Exception("How did it get here!"));
        }

        private static (bool success, int dmg, Exception exc) TankAttack(UnitType attacker, UnitType attacked)
        {
            var roll = _random.Next(100) + 1;
            int chance = 0;
            int dmg = 0;
            switch (attacker)
            {
                case UnitType.TankHeavy:
                    switch (attacked)
                    {
                        case UnitType.Infantry:
                            chance = HeavyTank_Soldier_Chance;
                            dmg = HeavyTank_Soldier_Dmg;
                            break;
                        case UnitType.PlaneBomber:
                            chance = HeavyTank_BomberPlane_Chance;
                            dmg = HeavyTank_BomberPlane_Dmg;
                            break;
                        case UnitType.PlaneFighter:
                            chance = HeavyTank_FighterPlane_Chance;
                            dmg = HeavyTank_FighterPlane_Dmg;
                            break;
                        case UnitType.TankHeavy:
                            chance = HeavyTank_HeavyTank_Chance;
                            dmg = HeavyTank_HeavyTank_Dmg;
                            break;
                        case UnitType.TankLight:
                            chance = HeavyTank_LightTank_Chance;
                            dmg = HeavyTank_LightTank_Dmg;
                            break;
                    }
                    break;
                case UnitType.TankLight:
                    switch (attacked)
                    {
                        case UnitType.Infantry:
                            chance = LightTank_Soldier_Chance;
                            dmg = LightTank_Soldier_Dmg;
                            break;
                        case UnitType.PlaneBomber:
                            chance = LightTank_BomberPlane_Chance;
                            dmg = LightTank_BomberPlane_Dmg;
                            break;
                        case UnitType.PlaneFighter:
                            chance = LightTank_FighterPlane_Chance;
                            dmg = LightTank_FighterPlane_Dmg;
                            break;
                        case UnitType.TankHeavy:
                            chance = LightTank_HeavyTank_Chance;
                            dmg = LightTank_HeavyTank_Dmg;
                            break;
                        case UnitType.TankLight:
                            chance = LightTank_LightTank_Chance;
                            dmg = LightTank_LightTank_Dmg;
                            break;
                    }
                    break;
            }
            if (roll <= chance)
                return (true, dmg, null);
            return (false, 0, null);
        }

        private static (bool success, int dmg, Exception exc) PlaneAttack(UnitType attacker, UnitType attacked)
        {
            var roll = _random.Next(100) + 1;
            int chance = 0;
            int dmg = 0;
            switch (attacker)
            {
                case UnitType.PlaneFighter:
                    switch (attacked)
                    {
                        case UnitType.Infantry:
                            chance = FighterPlane_Soldier_Chance;
                            dmg = FighterPlane_Soldier_Dmg;
                            break;
                        case UnitType.PlaneBomber:
                            chance = FighterPlane_BomberPlane_Chance;
                            dmg = FighterPlane_BomberPlane_Dmg;
                            break;
                        case UnitType.PlaneFighter:
                            chance = FighterPlane_FighterPlane_Chance;
                            dmg = FighterPlane_FighterPlane_Dmg;
                            break;
                        case UnitType.TankHeavy:
                            chance = FighterPlane_HeavyTank_Chance;
                            dmg = FighterPlane_HeavyTank_Dmg;
                            break;
                        case UnitType.TankLight:
                            chance = FighterPlane_LightTank_Chance;
                            dmg = FighterPlane_LightTank_Dmg;
                            break;
                    }
                    break;
                case UnitType.PlaneBomber:
                    switch (attacked)
                    {
                        case UnitType.Infantry:
                            chance = BomberPlane_Soldier_Chance;
                            dmg = BomberPlane_Soldier_Dmg;
                            break;
                        case UnitType.PlaneBomber:
                            chance = BomberPlane_BomberPlane_Chance;
                            dmg = BomberPlane_BomberPlane_Dmg;
                            break;
                        case UnitType.PlaneFighter:
                            chance = BomberPlane_FighterPlane_Chance;
                            dmg = BomberPlane_FighterPlane_Dmg;
                            break;
                        case UnitType.TankHeavy:
                            chance = BomberPlane_HeavyTank_Chance;
                            dmg = BomberPlane_HeavyTank_Dmg;
                            break;
                        case UnitType.TankLight:
                            chance = BomberPlane_LightTank_Chance;
                            dmg = BomberPlane_LightTank_Dmg;
                            break;
                    }
                    break;
            }
            if (roll <= chance)
                return (true, dmg, null);
            return (false, 0, null);
        }

        private static (bool success, int dmg, Exception exc) SoldierAttack(UnitType attacked)
        {
            var roll = _random.Next(100) + 1;
            int chance = 0;
            int dmg = 0;
            switch (attacked)
            {
                case UnitType.Infantry:
                    chance = Soldier_Soldier_Chance;
                    dmg = Soldier_Soldier_Dmg;
                    break;
                case UnitType.PlaneBomber:
                    chance = Soldier_BomberPlane_Chance;
                    dmg = Soldier_BomberPlane_Dmg;
                    break;
                case UnitType.PlaneFighter:
                    chance = Soldier_FighterPlane_Chance;
                    dmg = Soldier_FighterPlane_Dmg;
                    break;
                case UnitType.TankHeavy:
                    chance = Soldier_HeavyTank_Chance;
                    dmg = Soldier_HeavyTank_Dmg;
                    break;
                case UnitType.TankLight:
                    chance = Soldier_LightTank_Chance;
                    dmg = Soldier_LightTank_Dmg;
                    break;
            }
            if (roll <= chance)
                return (true, dmg, null);
            return (false, 0, null);
        }
    }
}
