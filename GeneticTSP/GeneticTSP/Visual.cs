using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeneticTSP
{
    public partial class Visual : Form
    {
        static Graph g;
        static Pool tadpoles;

        readonly Pen p = new Pen(Color.Black);

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

        void Draw(object s, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;

            Route bestPath = tadpoles.routes[0];

            for (int i = 0; i < bestPath.Stops.Count - 1; i++)
            {
                g.DrawLine(p, (int) bestPath.Stops[i].x, (int) bestPath.Stops[i].y, (int) bestPath.Stops[i + 1].x,
                    (int) bestPath.Stops[i + 1].y);
            }

            g.DrawLine(p, (int) bestPath.Stops[0].x, (int) bestPath.Stops[0].y,
                (int) bestPath.Stops[bestPath.Stops.Count - 1].x, (int) bestPath.Stops[bestPath.Stops.Count - 1].y);
        }

        void Next(object s, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                tadpoles.Breed();
            }

            Console.WriteLine("Same as best: " + tadpoles.SameAsBest());
            Console.WriteLine("Current score: " + tadpoles.routes[0].Fitness);
            Invalidate();
        }
    }
}