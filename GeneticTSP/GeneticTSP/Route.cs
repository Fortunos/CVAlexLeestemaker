using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticTSP
{
    class Route
    {
        public List<Node> stops = new List<Node>();
        public double fitness;

        public Route(List<Node> nodes, bool random)
        {
            if (random)
            {
                GenerateRandomRoute(nodes);
            }
            else
            {
                this.stops = nodes;
            }
            fitness = GetFitness();
        }

        public void GenerateRandomRoute(List<Node> nodes)
        {
            Random rnd = new Random();
            bool[] picked = new bool[nodes.Count];
            while (stops.Count < nodes.Count)
            {
                int r = rnd.Next(nodes.Count);
                if (!picked[r])
                {
                    stops.Add(nodes[r]);
                    picked[r] = true;
                }
            }
        }

        //higher fitness = worse I know confusing right get over it
        public double GetFitness()
        {
            double result = 0;

            for (int i = 0; i < stops.Count - 1; i++)
            {
                result += Distance(stops[i], stops[i + 1]);
            }

            return result;
        }

        private double Distance(Node a, Node b)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2)));
        }

        public static bool operator ==(Route a, Route b)
        {
            for (int i = 0; i < a.stops.Count; i++)
            {
                if (a.stops[i].x == b.stops[i].x && a.stops[i].y == b.stops[i].y)
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
