using System;

namespace Logmaku.Utils
{
    public static class RandomHelper
    {
        private static readonly Random Rand = new Random();

        public static double Random(double min, double max) => Rand.Next((int) min, (int) max);
    }
}