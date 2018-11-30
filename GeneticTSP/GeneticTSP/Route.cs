using System;
using System.Collections.Generic;

namespace GeneticTSP
{
    internal class Route
    {
        public List<Node> Stops = new List<Node>();
        public double Fitness;

        public Route(List<Node> nodes, bool random)
        {
            if (random)
            {
                GenerateRandomRoute(nodes);
            }
            else
            {
                Stops = nodes;
            }
            Fitness = GetFitness();
        }

        public void GenerateRandomRoute(List<Node> nodes)
        {
            Random rnd = new Random();
            bool[] picked = new bool[nodes.Count];
            while (Stops.Count < nodes.Count)
            {
                int r = rnd.Next(nodes.Count);
                if (!picked[r])
                {
                    Stops.Add(nodes[r]);
                    picked[r] = true;
                }
            }
        }

        // Fitness reflects the length of a route, so a lower fitness is better
        public double GetFitness()
        {
            double result = 0;

            for (int i = 0; i < Stops.Count - 1; i++)
            {
                result += Distance(Stops[i], Stops[i + 1]);
            }

            return result;
        }

        double Distance(Node a, Node b)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2)));
        }

        public static bool operator ==(Route a, Route b)
        {
            for (int i = 0; i < a.Stops.Count; i++)
            {
                if (a.Stops[i].x == b.Stops[i].x && a.Stops[i].y == b.Stops[i].y)
                { }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Route a, Route b)
        {
            return !(a == b);
        }
    }
}
