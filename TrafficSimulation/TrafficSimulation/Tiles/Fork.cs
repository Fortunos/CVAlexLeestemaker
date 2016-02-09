using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Fork : Tile
    {
        /*
         * lanes is and array with all the lanes
         * control is the trafficlightcontrol with the trafficlights used on the tile
         * notDirection is only used in Fork and returns the direction on which there is no connection
         */
        public int[] lanes;
        public TrafficlightControl control;
        public int notDirection;

        /// <summary>
        /// Constructor used by fork, based on the constructor in Tile
        /// here all default values of the Fork are set.
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="notDirection"></param>
        public Fork(SimControl sim, int notDirection)
        {
            this.name = "Fork";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            this.notDirection = notDirection;
            lanes[notDirection * 2 - 2] = 0;
            lanes[notDirection * 2 - 1] = 0;
            directions.Add(1);
            directions.Add(2);
            directions.Add(3);
            directions.Add(4);
            directions.Remove(notDirection);
            control = new TrafficlightControl(sim, this, 3, notDirection, lanes);
            int totalLanes = CountLanes(lanes);
            Initialize();
        }

        /// <summary>
        /// Based on the method GetLanesIn in Tile
        /// returns the lanes going into the tile in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesIn(int direction)
        {
            int value;
            value = lanes[direction * 2 - 2];
            if (value != 0)
                return value;
            else
                return 1;
        }

        /// <summary>
        /// Based on the method GetLanesOut in Tile
        /// returns the lanes going out of the tile in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesOut(int direction)
        {
            int value;
            value = lanes[direction * 2 - 1];
            if (value != 0)
                return value;
            else
                return 1;
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
            if (directions.Contains(direction))
            {
                lanes[direction * 2 - 1] = lanesOut;
                lanes[direction * 2 - 2] = lanesIn;
            }
            control = new TrafficlightControl(s, this, 3, NotDirection, lanes, position);
        }

        /// <summary>
        /// Based on the method GetControl in Tile
        /// returns the TrafficLightControl used by the Fork.
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
            if ((side + 1) % 4 + 1 != NotDirection)
                return true;
            return false;
        }

        /// <summary>
        /// Based on the method DrawImage in Tile
        /// returns the bitmap of the Fork.
        /// </summary>
        /// <returns></returns>
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawForkroad(Graphics.FromImage(image), lanes);
            return image;
        }

        /// <summary>
        /// Based on the method SetValues in Tile
        /// sets the values given to the Fork when the Fork is placed on the map.
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