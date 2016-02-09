using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class SelectTile : Tile
    {
        /// <summary>
        /// Based on the method DrawImage in Tile
        /// returns the bitmap of the tile with lightblue borders.
        /// </summary>
        /// <returns></returns>
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSelectTile(Graphics.FromImage(image));
            return image;
        }

        /// <summary>
        /// Based on the method GetLanesin in Tile
        /// always returns 0.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesIn(int direction)
        {
            return 0;
        }

        /// <summary>
        /// Based on the method GetLanesOut in Tile
        /// always returns 0.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesOut(int direction)
        {
            return 0;
        }

        /// <summary>
        /// Based on the method GetControl in Tile
        /// returns the TrafficLightControl used by the SelectTile.
        /// </summary>
        /// <returns></returns>
        public override TrafficlightControl GetControl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Based on the method doesConnect in Tile
        /// always returns false.
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public override bool doesConnect(int side)
        {
            return false;
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

        }
    }
}
