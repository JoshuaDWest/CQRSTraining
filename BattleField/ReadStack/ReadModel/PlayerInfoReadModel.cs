using Models.Enums;

namespace ReadStack.ReadModel
{
    public class PlayerInfoReadModel
    {
        public void Update(PlayerInfoReadModel other)
        {
            if (other.HeavyTankCount != null)
                HeavyTankCount?.Update(other.HeavyTankCount);
            if (other.LightTankCount != null)
                HeavyTankCount?.Update(other.LightTankCount);
            if (other.FighterPlaneCount != null)
                HeavyTankCount?.Update(other.FighterPlaneCount);
            if (other.BomberCount != null)
                HeavyTankCount?.Update(other.BomberCount);
            if (other.InfantryCount != null)
                HeavyTankCount?.Update(other.InfantryCount);
        }

        public UnitTypeCountReadModel HeavyTankCount { get; set; }
        public UnitTypeCountReadModel LightTankCount { get; set; }
        public UnitTypeCountReadModel FighterPlaneCount { get; set; }
        public UnitTypeCountReadModel BomberCount { get; set; }
        public UnitTypeCountReadModel InfantryCount { get; set; }

        public static PlayerInfoReadModel New => new PlayerInfoReadModel
        {
            HeavyTankCount = new UnitTypeCountReadModel(),
            LightTankCount = new UnitTypeCountReadModel(),
            FighterPlaneCount = new UnitTypeCountReadModel(),
            BomberCount = new UnitTypeCountReadModel(),
            InfantryCount = new UnitTypeCountReadModel()
        };

        public class UnitTypeCountReadModel
        {
            public void Update(UnitTypeCountReadModel other)
            {
                Count = other.Count;
            }

            public UnitType UnitType { get; internal set; }
            public int Count { get; set; }
        }
    }
}
