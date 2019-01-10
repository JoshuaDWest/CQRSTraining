using System;

namespace CommandStack.Utilities
{
    public class BattleRandomizer : IRandomizer
    {
        private static Random _random = new Random((int)DateTime.Now.Ticks);

        public int Generate(int max)
        {
            return _random.Next(max);
        }

        public int Generate(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
