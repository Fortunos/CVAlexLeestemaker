using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticTSP
{
    class Graph
    {
        public List<Node> nodes = new List<Node>();

        public void GenerateNodes(int amount)
        {
            Random rnd = new Random();

            for (int i = 0; i < amount; i++)
            {
                nodes.Add(new Node(rnd.Next(501), rnd.Next(501)));
            }
        }

        public double Distance(Node a, Node b)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2)));
        }
    }
}
