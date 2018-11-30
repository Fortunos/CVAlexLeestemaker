using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticTSP
{
    internal class Pool
    {
        public List<Route> routes = new List<Route>();

        const float mut = 0.8f;
        const int poolSize = 100;
        const int childPoolSize = 1000;
        const int maxLoops = 50;

        public Pool(List<Node> nodes)
        {
            for (int i = 0; i < poolSize; i++)
            {
                routes.Add(new Route(nodes, true));
                routes.Sort(sortByFitness);
            }
        }

        public void Breed()
        {
            List<Route> children = new List<Route>();

            Route[] threadsafe = new Route[childPoolSize];

            Parallel.For(0, childPoolSize, i =>
            {
                int x = StaticRandom.Rand(routes.Count);
                int y = StaticRandom.Rand(routes.Count);

                Route child = Combine(routes[x], routes[y]);
                threadsafe[i] = child;
            });

            children = threadsafe.ToList();

            children.Sort(sortByFitness);
            children.RemoveRange(poolSize, childPoolSize - poolSize);

            routes = children;
        }

        public int SameAsBest()
        {
            return routes.Count(r => r == routes[0]);
        }

        public Route Combine(Route a, Route b)
        {
            List<Node> stops = new List<Node>();

            int split = StaticRandom.Rand(a.Stops.Count);

            //pure breeding
            for (int i = 0; i < split; i++)
            {
                stops.Add(a.Stops[i]);
            }

            for (int i = 0; i < b.Stops.Count; i++)
            {
                Node next = b.Stops[i];
                if (!stops.Contains(next))
                {
                    stops.Add(next);
                }
            }

            stops = SwapMutate(stops, 0);

            return new Route(stops, false);
        }

        List<Node> SwapMutate(List<Node> stops, int loops)
        {
            if (StaticRandom.RandDouble() < mut && loops < maxLoops)
            {
                int x = StaticRandom.Rand(stops.Count);
                int y = StaticRandom.Rand(stops.Count);

                Node temp = stops[x];
                stops[x] = stops[y];
                stops[y] = temp;
                stops = SwapMutate(stops, loops + 1);
            }

            return stops;
        }

        int sortByFitness(Route a, Route b)
        {
            return a.Fitness.CompareTo(b.Fitness);
        }
    }
}