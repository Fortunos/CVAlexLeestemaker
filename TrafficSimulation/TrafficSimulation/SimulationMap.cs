using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TrafficSimulation
{
    public class SimulationMap
    {
        private Tile[,] map;
        private SimControl simControl;
        private int smallX, smallY;
        private int largeX, largeY;
        private List<Tile> tileList;

        public SimulationMap(SimControl simControl)
        {
            this.simControl = simControl;
            tileList = new List<Tile>();
        }
        //maakt de map, word aangeroepen als de simulatie word gestart
        public void CreateMap()
        {
            CreateList(tileList);
        }

        public Tile[,] Map { get { return map; } }

        //is nodig als een map wordt geladen, dan word de lijst van tiles ingelezen
        private void CreateList(List<Tile> tileList)
        {
            createArray(tileList);

            foreach (Tile t in tileList)
            {
                if (t != null)
                {
                    map[ToGrid(t.position).X, ToGrid(t.position).Y] = t;
                }
            }
        }

        //maakt een array van de lijst met alle tiles erin
        private void createArray(List<Tile> tileList)
        {
            smallX = 0;
            largeX = 0;
            smallY = 0;
            largeY = 0;

            bool first = true;

            foreach (Tile t in tileList)
            {
                if (t != null && first == true)
                {
                    smallX = t.position.X;
                    largeX = t.position.X;
                    smallY = t.position.Y;
                    largeY = t.position.Y;

                    first = false;
                }
                else
                {
                    if (t != null)
                    {
                        if (t.position.X < smallX)
                        {
                            smallX = t.position.X;
                        }
                        else if (t.position.X > largeX)
                        {
                            largeX = t.position.X;
                        }

                        if (t.position.Y < smallY)
                        {
                            smallY = t.position.Y;
                        }
                        else if (t.position.Y > largeY)
                        {
                            largeY = t.position.Y;
                        }
                    }
                }
            }

            smallX = smallX / 100;
            smallY = smallY / 100;
            largeX = largeX / 100;
            largeY = largeY / 100;
            map = new Tile[largeX - smallX + 1, largeY - smallY + 1];
        }

        //lijst met alle tiles erin
        public List<Tile> GetMap()
        {
            return tileList;
        }

        public Tile GetTileAbove(Point position)
        {
            if (ToGrid(position).Y > 0)
            {
                return map[ToGrid(position).X, ToGrid(position).Y - 1];
            }
            else
            {
                return null;
            }
        }

        public Tile GetTileBelow(Point position)
        {
            if (ToGrid(position).Y < Map.GetLength(1) - 1)
            {
                return map[ToGrid(position).X, ToGrid(position).Y + 1];
            }
            else
            {
                return null;
            }
        }

        public Tile GetTileLeft(Point position)
        {
            if (ToGrid(position).X > 0)
            {
                return map[ToGrid(position).X - 1, ToGrid(position).Y];
            }
            else
            {
                return null;
            }
        }

        public Tile GetTileRight(Point position)
        {
            if (ToGrid(position).X < Map.GetLength(0) - 1)
            {
                int x = ToGrid(position).X;
                int y = ToGrid(position).Y;

                return map[ToGrid(position).X + 1, ToGrid(position).Y];
            }
            else
            {
                return null;
            }
        }
        //deze wordt gebruikt als de simulatie is gestart
        public Tile[] GetSurroundingTilesSim(Point pos)
        {
            //createMap has to have been called
            Tile[] tileArray = { GetTileAbove(pos), GetTileRight(pos), GetTileBelow(pos), GetTileLeft(pos) };
            return tileArray;
        }
        //deze wordt gebruikt als de simulatie nog niet is gestart
        public Tile[] GetSurroundingTiles(Point pos)
        {
            Tile[] tileArray = new Tile[4];

            foreach (Tile t in tileList)
            {
                if (t.position.X == pos.X)
                {
                    if (t.position.Y == pos.Y - 100)
                    {
                        tileArray[0] = t;
                    }
                    else if (t.position.Y == pos.Y + 100)
                    {
                        tileArray[2] = t;
                    }
                }
                if (t.position.Y == pos.Y)
                {
                    if (t.position.X == pos.X - 100)
                    {
                        tileArray[3] = t;
                    }
                    else if (t.position.X == pos.X + 100)
                    {
                        tileArray[1] = t;
                    }
                }
            }

            return tileArray;
        }

        public Tile[] GetConnectingTiles(Point pos)
        {
            Tile[] connectingTiles = new Tile[4];

            foreach (int d in GetTile(pos).Directions)
            {
                if (GetSurroundingTiles(pos)[d - 1] != null)
                {
                    connectingTiles[d - 1] = GetSurroundingTiles(pos)[d - 1];
                }
            }

            return connectingTiles;
        }

        //returnt het gegeven punt als een punt op de grid, als de array word aangemaakt dan is deze veel kleiner als de originele array
        public Point ToGrid(Point p)
        {
            int temp1 = p.X / 100;
            int temp2 = p.Y / 100;

            Point point = new Point((p.X / 100) - smallX, (p.Y / 100) - smallY);
            return point;
        }

        //is nodig als er een muisclick word gedaan, hierzo word deze click verbonden met een punt op de grid.
        public Point GetPosition(Point p)
        {
            return new Point((p.X / 100) * 100, (p.Y / 100) * 100);
        }

        public void RemoveTile(Tile t)
        {
            tileList.Remove(t);
        }

		public void ClearTileList()
		{
			tileList.Clear();
		}

        //returned een tile aan de hand van een x en y
        public Tile GetTileMea(int x, int y)
        {
            Point p = GetPosition(new Point(x, y));

            foreach (Tile t in tileList)
            {
                if (t.position == p)
                {
                    return t;
                }
            }
            return null;
        }

        //returned een tile aan de hand van een positie
        public Tile GetTile(Point pos)
        {
            foreach (Tile t in tileList)
            {
                if (t.position == pos)
                {
                    return t;
                }
            }
            return null;
        }

        public void AddTile(Tile t)
        {
            foreach (Tile tile in tileList)
            {
                if (t.position == tile.position)
                {
                    RemoveTile(tile);
                    tileList.Add(t);
                    break;
                }
            }
            tileList.Add(t);
        }
        public void ResetCarsOnTile()
        {
            foreach(Tile t in tileList)
            {
                t.NumberOfVehicles = 0;
            }
        }
    }
}