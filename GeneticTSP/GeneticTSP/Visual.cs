using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticTSP
{
    public partial class Visual : Form
    {
        private static Graph g;
        private static Pool tadpoles;

        private Pen p = new Pen(Color.Black);

        public Visual()
        {
            InitializeComponent();

            ClientSize = new Size(500, 500);

            Paint += Draw;
            MouseClick += Next;
            
            g = new Graph();
            g.GenerateNodes(100);
            tadpoles = new Pool(g.nodes);
        }

        private void Draw(object s, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            Route bestPath = tadpoles.routes[0];

            for (int i = 0; i < bestPath.stops.Count - 1; i++)
            {
                g.DrawLine(p, (int) bestPath.stops[i].x, (int) bestPath.stops[i].y, (int) bestPath.stops[i + 1].x,
                    (int) bestPath.stops[i + 1].y);
            }
            g.DrawLine(p, (int) bestPath.stops[0].x, (int) bestPath.stops[0].y, (int) bestPath.stops[bestPath.stops.Count - 1].x, (int)bestPath.stops[bestPath.stops.Count - 1].y);
        }

        private void Next(object s, EventArgs e)
        {
            for (int i=0;i<100;i++)
            {
                //Console.WriteLine(tadpoles.routes[0].fitness);
                tadpoles.Breed();
            }
            Console.WriteLine("Same as best: " + tadpoles.SameAsBest());
            Console.WriteLine("Current score: " + tadpoles.routes[0].fitness);
            Invalidate();
        }
    }
}
