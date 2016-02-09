using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Crossroad : Tile
    {
        /*
         * lanes is and array with all the lanes
         * control is the trafficlightcontrol with the trafficlights used on the tile
         */
        int[] lanes;
        public TrafficlightControl control;

        /// <summary>
        /// Constructor used by Crossroad, based on the constructor in Tile
        /// here all default values of the Crossroad are set.
        /// </summary>
        /// <param name="sim"></param>
        public Crossroad(SimControl sim)
        {
            this.name = "Crossroad";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

            control = new TrafficlightControl(sim, this, 4, 5, lanes);
            directions.Add(1);
            directions.Add(2);
            directions.Add(3);
            directions.Add(4);
            int totalLanes = CountLanes(lanes);
            Initialize();
        }

        /// <summary>
        /// Based on the method GetLanesIn in Tile
        /// returns the lanes going into the Crossroad in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesIn(int direction)
        {
            return lanes[direction * 2 - 2];
        }

        /// <summary>
        /// Based on the method GetLanesOut in Tile
        /// returns the lanes going out of the Crossroad in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesOut(int direction)
        {
            return lanes[direction * 2 - 1];
        }

        /// <summary>
        /// Based on the method UpdateLanes in Tile
        /// this method is called when the lanes are updated.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="direction"></param>
        /// <param name="lanesIn"></param>
        /// <param name="lanesOut"></param>
        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            lanes[direction * 2 - 1] = lanesOut;
            lanes[direction * 2 - 2] = lanesIn;
            control = new TrafficlightControl(s, this, 4, 5, lanes, position);
        }

        /// <summary>
        /// Based on the method GetControl in Tile
        /// returns the TrafficLightControl used by the Crossroad.
        /// </summary>
        /// <returns></returns>
        public override TrafficlightControl GetControl()
        {
            return control;
        }

        /// <summary>
        /// Based on the method doesConnect in Tile
        /// returns true if the given side connects to another tile and false if it doesn't.
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public override bool doesConnect(int side)
        {
            return true;
        }

        /// <summary>
        /// Based on the method DrawImage in Tile
        /// returns the bitmap of the Crossroad.
        /// </summary>
        /// <returns></returns>
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawCrossroad(Graphics.FromImage(image), lanes);
            return image;
        }

        /// <summary>
        /// Based on the method SetValues in Tile
        /// sets the values given to the Crossroad when the Crossroad is placed on the map.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);
            control.ChangeValues(position);
        }
    }
}
