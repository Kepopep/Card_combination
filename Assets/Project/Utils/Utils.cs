using System;

namespace Project.Utils
{
    internal static class Utils
    {
        private static Random _random = new Random();

        public static int RandomInt(int fromIncluded, int toIncluded)
        {
            return _random.Next(fromIncluded, toIncluded + 1);
        }

        public static int RandomInt(int toIncluded)
        {
            return _random.Next(toIncluded + 1);
        }

        public static float RandomFloat()
        {
            return (float)_random.NextDouble();
        }
    }
}
