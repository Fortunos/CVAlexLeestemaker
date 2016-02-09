using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class removeTile : Tile
    {
        /// <summary>
        /// Based on the method DrawImage in Tile
        /// returns an empty bitmap.
        /// </summary>
        /// <returns></returns>
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawRemoveTile(Graphics.FromImage(image));
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
        /// Based on the method UpdateLanes in Tile.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="direction"></param>
        /// <param name="lanesIn"></param>
        /// <param name="lanesOut"></param>
        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {

        }
        /// <summary>
        /// Based on the method SetValues in Tile.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        public override void SetValues(SimControl s, Point position)
        {

        }
    }
}
