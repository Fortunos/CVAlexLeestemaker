using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace TrafficSimulation
{
    class Trafficlight
    {
        Color color;
        Point Position;
        Tile road;
        SimControl sc;
        public int LaneType;

        //brushes
        Brush zwart = new SolidBrush(Color.Black);
        Brush groen = new SolidBrush(Color.Green);
        Brush rood = new SolidBrush(Color.Red);
        Brush oranje = new SolidBrush(Color.Orange);

        public Trafficlight(SimControl sim, Tile road, Point Position, int LaneType)
        {
            this.LaneType = LaneType;
            this.road = road;
            this.Position = Position;
            sc = sim;
        }

        public Color Kleur
        {
            get { return color; }
        }

        public void UpdateColor(Color kleur)
        {
            //update the member-variable and actually draw the light
            DrawTrafficlight(kleur);
            color = kleur;
        }

        public void DrawTrafficlight(Color kleur)
        {
            Graphics gr = sc.trafficlightBC.GetBitmapGraphics;
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            //actual position of the trafficlight
            Point TruePos = new Point(Position.X + road.position.X, Position.Y + road.position.Y);

            //draw black rectangle
            gr.FillRectangle(zwart, TruePos.X, TruePos.Y, 10, 10);
            //draw the light itself
            if (kleur == Color.Green)
            {
                gr.FillEllipse(groen, TruePos.X + 1, TruePos.Y + 1, 8, 8);
            }
            else if (kleur == Color.Red)
            {
                gr.FillEllipse(rood, TruePos.X + 1, TruePos.Y + 1, 8, 8);
            }
            else if (kleur == Color.Orange)
            {
                gr.FillEllipse(oranje, TruePos.X + 1, TruePos.Y + 1, 8, 8);
            }
        }
    }
}