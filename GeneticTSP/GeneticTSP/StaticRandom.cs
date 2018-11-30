using System;
using System.Threading;

namespace GeneticTSP
{
    public static class StaticRandom
    {
        static readonly Random _global = new Random();
        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => {
                int seed;
                lock (_global) seed = _global.Next();
                return new Random(seed);
            });

        public static int Rand()
        {
            return random.Value.Next();
        }

        public static int Rand(int max)
        {
            return random.Value.Next(max);
        }

        public static double RandDouble()
        {
            return random.Value.NextDouble();
        }
    }
}
