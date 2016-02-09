using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Road : Tile
    {
        /// <summary>
        /// Constructor used by Road, based on the constructor in Tile
        /// here all default values of the Road are set.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Road(int start, int end)
        {
            this.name = "Road";

            if (start < end)
            {
                directions.Add(start);
                directions.Add(end);
            }
            else
            {
                directions.Add(end);
                directions.Add(start);
            }
            
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
            if (direction == StartDirection)
                return lanesLowToHigh;
            else if (direction == EndDirection)
                return lanesHighToLow;
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
            if (direction == StartDirection)
                return lanesHighToLow;
            else if (direction == EndDirection)
                return lanesLowToHigh;
            else
                return 1;
        }

        /// <summary>
        /// Based on the method GetControl in Tile
        /// returns the TrafficLightControl used by the Road.
        /// </summary>
        /// <returns></returns>
        public override TrafficlightControl GetControl()
        {
            throw new NotImplementedException();
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
            if (direction == StartDirection)
            {
                this.lanesLowToHigh = lanesIn;
                this.lanesHighToLow = lanesOut;
            }
            if (direction == EndDirection)
            {


                this.lanesLowToHigh = lanesOut;
                this.lanesHighToLow = lanesIn;
            }
        }

        /// <summary>
        /// Based on the method doesConnect in Tile
        /// returns true if the given side connects to another tile and false if it doesn't.
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public override bool doesConnect(int side)
        {
            int direction = (side + 1) % 4 + 1;
            if (direction == StartDirection || direction == EndDirection)
                return true;
            return false;
        }

        /// <summary>
        /// Based on the method DrawImage in Tile
        /// returns the bitmap of the Road.
        /// </summary>
        /// <returns></returns>
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawRoad(Graphics.FromImage(image), lanesLowToHigh, lanesHighToLow, StartDirection, EndDirection);
            return image;
        }
    }
}
