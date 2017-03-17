using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticTSP
{
    class Program
    {
        private static Graph g;
        private static Pool tadpoles;

        static void Main()
        {
            //g = new Graph();
            //g.GenerateNodes(100);
            //tadpoles = new Pool(g.nodes);

            //while (true)
            //{
            //    Console.WriteLine(tadpoles.routes[0].fitness);
            //    tadpoles.Breed();
            //}


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Visual());
        }
    }
}
