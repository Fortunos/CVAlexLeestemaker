using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTSP
{
    class Pool
    {
        public List<Route> routes = new List<Route>();

        private float mut = 0.8f;
        private int poolSize = 100;
        private int childPoolSize = 1000;
        private int maxLoops = 50;

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

            //while (children.Count < childPoolSize)
            //{
            //    int x = rnd.Next(routes.Count);
            //    int y = rnd.Next(routes.Count);

            //    Route child = Combine(routes[x], routes[y]);
            //    children.Add(child);
            //}

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

            int split = StaticRandom.Rand(a.stops.Count);

            //pure breeding
            for (int i = 0; i < split; i++)
            {
                stops.Add(a.stops[i]);
            }

            for (int i = 0; i < b.stops.Count; i++)
            {
                Node next = b.stops[i];
                if (!stops.Contains(next))
                {
                    stops.Add(next);
                }
            }

            //mutation through swap
            //if (rnd.NextDouble() < mut)
            //{
            //    int x = rnd.Next(a.stops.Count);
            //    int y = rnd.Next(a.stops.Count);

            //    Node temp = stops[x];
            //    stops[x] = stops[y];
            //    stops[y] = temp;
            //}
            stops = SwapMutate(stops, 0);

            return new Route(stops, false);
        }

        private List<Node> SwapMutate(List<Node> stops, int loops)
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

        private int sortByFitness(Route a, Route b)
        {
            if (a.fitness > b.fitness)
                return 1;
            if (b.fitness > a.fitness)
                return -1;
            return 0;
        }
    }
}
