using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TrafficSimulation
{
    public abstract class Tile
    {
        /*
         * Variables of tile
         *      name is the name of the tile, used to determine what kind of tile the tile is
         *      position is the position of the tile, in pixels
         *      maxSpeed is the speed cars are moving on the tile
         *      size is the size of the tile, which is 100,100
         *      
         *      access is a boolean array[,],  each lane has a boolean. If the boolean is true cars can enter the lane and if it's false cars can't enter the lane
         *      lanesLowToHigh is the number of lanes from the lowest direction to the highest direction
         *      lanesHighToLow is the number of lanes from the highest direction to the lowest direction
         *      directions is a list with 
         *      
         *      numberOfVehicles are the number of vehicles that are currently on the tile
         */
        //tilevariables
        public string name;
        public Point position;
        public int maxSpeed;
        public Size size;
        //simulatievariables
        public List<List<Vehicle>>[] vehicles;
        protected bool[,] access;
        protected int lanesLowToHigh;
        protected int lanesHighToLow;
        protected List<int> directions;
        //interfacevariables
        protected int numberOfVehicles;

        /// <summary>
        /// Returns the directions of the tile
        /// it returns one direction because by knowing which direction the tile doesnt have you als know which directions it does have.
        /// </summary>
        public int NotDirection
        {
            get
            {
                if (!directions.Contains(1))
                {
                    return 1;
                }
                else if (!directions.Contains(2))
                {
                    return 2;
                }
                else if (!directions.Contains(3))
                {
                    return 3;
                }
                else if (!directions.Contains(4))
                {
                    return 4;
                }
                else
                {
                    return 0;
                }
            }
        }

        /*
         * returns the startdirection of the tile, used by road
         * return the enddirection of the tile, used by road
         */
        public int StartDirection { get { return directions.ElementAt(0); } }
        public int EndDirection { get { return directions.ElementAt(1); } }

        /*
         * returns the direction the spawner is facing, used by spawner
         */
        public int Direction { get { return directions.ElementAt(0); } }

        /// <summary>
        /// Tile constructor, no variables are given
        /// all values are edited with setters, when the tile is placed all values are set to their default values.
        /// </summary>
        public Tile()
        {
            this.MaxSpeed = 2;
            this.LanesHighToLow = 1;
            this.LanesLowToHigh = 1;
            this.size = new Size(100, 100);
            directions = new List<int>();
            Initialize();
        }
        /*
         * getters and setters needed to run the simulation and save the tiles
         */
        public List<int> Directions { get { return this.directions; } }
        public int NumberOfVehicles { get { return numberOfVehicles; } set { numberOfVehicles = value; } }
        public bool[,] Access { get { return access; } set { access = value; } }
        public int LanesHighToLow { get { return this.lanesHighToLow; } set { lanesHighToLow = value; } }
        public int LanesLowToHigh { get { return this.lanesLowToHigh; } set { lanesLowToHigh = value; } }
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

        /// <summary>
        /// Called when the tile is made and sets all tile values to the default values and instantiates vehicles and access.
        /// </summary>
        public void Initialize()
        {
            vehicles = new List<List<Vehicle>>[4];
            access = new bool[4, 3];
            for (int i = 0; i < 4; i++)
            {
                vehicles[i] = new List<List<Vehicle>>();
                for (int j = 0; j < 3; j++)
                {
                    access[i, j] = true;
                    vehicles[i].Add(new List<Vehicle>());
                }

            }
        }

        /// <summary>
        /// Returns the bitmap of the tile, used when drawing the tile on the map.
        /// </summary>
        /// <returns>Tile.bitmap</returns>
        public abstract Bitmap DrawImage();

        /// <summary>
        /// Changes the lanes on a tile when a tile bordering this tile updates it's lanes, and they update the tiles bordering those tiles ect.
        /// the update chain stops at forks and crossroads.
        /// The method uses the lanes on the lowest direction of the placed tile, so the upper side first and the left side last.
        /// </summary>
        /// <param name="simcontrol"></param>
        /// <param name="direction"></param>
        public void UpdateFromOtherTile(SimControl simcontrol, int direction)
        {
            if (direction == 0)
            {
                int ForkNotDirection = 0;
                foreach (int d in directions)
                {
                    if (d != ForkNotDirection)
                    {
                        Tile nextTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[d - 1];
                        if (nextTile != null && nextTile.doesConnect(d))
                        {
                            this.UpdateLanes(simcontrol, d, nextTile.GetLanesOut((d + 1) % 4 + 1), nextTile.GetLanesIn((d + 1) % 4 + 1));
                            if (this.name == "Road")
                            {
                                Road roadTile = (Road)this;
                                int CounterDirection;
                                if (d == roadTile.StartDirection)
                                    CounterDirection = EndDirection;
                                else
                                    CounterDirection = StartDirection;
                                Tile otherTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[CounterDirection - 1];
                                if (otherTile != null)
                                {
                                    if (this.GetLanesOut(CounterDirection) != otherTile.GetLanesIn((CounterDirection + 1) % 4 + 1) || otherTile.GetLanesOut((CounterDirection + 1) % 4 + 1) != this.GetLanesIn(CounterDirection))
                                    {
                                        otherTile.UpdateLanes(simcontrol, (CounterDirection + 1) % 4 + 1, this.GetLanesOut(CounterDirection), this.GetLanesIn(CounterDirection));
                                        otherTile.UpdateOtherTiles(simcontrol, (CounterDirection + 1) % 4 + 1);
                                        break;
                                    }
                                }
                            }
                            if (this.name == "Fork")
                            {
                                Fork forkTile = (Fork)this;
                                if ((d == (forkTile.NotDirection + 2) % 4 + 1 || d == (forkTile.NotDirection) % 4 + 1))
                                {
                                    ForkNotDirection = (d + 1) % 4 + 1;
                                    forkTile.UpdateLanes(simcontrol, ForkNotDirection, this.GetLanesOut(d), this.GetLanesIn(d));
                                    Tile otherTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[ForkNotDirection - 1];
                                    if (otherTile != null)
                                    {
                                        otherTile.UpdateLanes(simcontrol, d, this.GetLanesIn(d), this.GetLanesOut(d));
                                        otherTile.UpdateOtherTiles(simcontrol, d);
                                    }

                                }
                            }
                        }

                    }
                }
            }
            simcontrol.backgroundBC.AddObject(this.DrawImage(), this.position);
        }

        /// <summary>
        /// This method is used when a new tile is placed on the map, this causes the tile to update its lanes according to bordering tiles.
        /// </summary>
        /// <param name="simcontrol"></param>
        /// <param name="NotDirection"></param>
        public void UpdateOtherTiles(SimControl simcontrol, int NotDirection)
        {
            if (this.name != "Crossroad")
            {
                if (this.name != "Fork")
                {
                    foreach (int d in directions)
                    {
                        if (d != NotDirection)
                        {
                            int CounterDirection = (d + 1) % 4 + 1;
                            Tile nextTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[d - 1];
                            if (nextTile != null)
                            {
                                if (this.GetLanesOut(d) != nextTile.GetLanesIn(CounterDirection) || nextTile.GetLanesOut(CounterDirection) != this.GetLanesIn(d))
                                {
                                    nextTile.UpdateLanes(simcontrol, CounterDirection, this.GetLanesOut(d), this.GetLanesIn(d));
                                    nextTile.UpdateOtherTiles(simcontrol, CounterDirection);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Fork forkTile = (Fork)this;
                    if ((NotDirection == (forkTile.NotDirection + 2) % 4 + 1 || NotDirection == (forkTile.NotDirection) % 4 + 1))
                    {
                        int ForkNotDirection = (NotDirection + 1) % 4 + 1;
                        forkTile.UpdateLanes(simcontrol, ForkNotDirection, this.GetLanesOut(NotDirection), this.GetLanesIn(NotDirection));
                        Tile otherTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[ForkNotDirection - 1];
                        if (otherTile != null)
                        {
                            otherTile.UpdateLanes(simcontrol, NotDirection, this.GetLanesIn(NotDirection), this.GetLanesOut(NotDirection));
                            otherTile.UpdateOtherTiles(simcontrol, NotDirection);
                        }

                    }
                }
            }
            simcontrol.backgroundBC.AddObject(this.DrawImage(), this.position);
        }

        /// <summary>
        /// Called when the lanes are updated.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="direction"></param>
        /// <param name="lanesIn"></param>
        /// <param name="lanesOut"></param>
        public abstract void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut);

        /// <summary>
        /// Returns the number of lanes going in to the tile in a specific direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public abstract int GetLanesIn(int direction);

        /// <summary>
        /// Returns the number of lanes going out of the tile in a specific direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public abstract int GetLanesOut(int direction);

        /// <summary>
        /// Returns true if the side connects to another tile and false if it doesn't.
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public abstract bool doesConnect(int side);

        /// <summary>
        /// Returns the TrafficlightControl of the tile, this control contains all the trafficlights used by the tile.
        /// </summary>
        /// <returns></returns>
        public abstract TrafficlightControl GetControl();

        /// <summary>
        /// Sets the values given to the tile when the tile is placed on the map.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        public virtual void SetValues(SimControl s, Point position)
        {
            this.position.X = (position.X / 100) * 100;
            this.position.Y = (position.Y / 100) * 100;
            this.UpdateFromOtherTile(s, 0);
        }

        /// <summary>
        /// Returns the total amount of lanes on a tile.
        /// </summary>
        /// <param name="lanes"></param>
        /// <returns></returns>
        public int CountLanes(int[] lanes)
        {
            int totalLanes = 0;
            for (int i = 0; i < lanes.Length; i++)
            {
                totalLanes += lanes[i];
            }
            return totalLanes;
        }

        /// <summary>
        /// Removes a vehicle on a tile, this is used when a vehicle leaves the map on a spawner
        /// also removes a vehicle when a vehicle leaves a tile to enter another.
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="v"></param>
        /// <param name="Side"></param>
        /// <param name="lane"></param>
        public void RemoveVehicle(SimControl sim, Vehicle v, int lastSide, int Side, int lane)
        {
            List<List<Vehicle>> sideVehicles = vehicles[lastSide - 1];
            List<Vehicle> laneVehicles = sideVehicles[lane];
            laneVehicles.Remove(v);
            numberOfVehicles--;
            //looks if there is space for other cars to come on the tile
            if (laneVehicles.Count < 5 && this.name != "Spawner" && this.name != "Crossroad" && this.name != "Fork")
            {
                Tile lastTile = sim.simulationMap.GetSurroundingTilesSim(this.position)[(lastSide + 1) % 4];
                if (lastTile != null)
                    lastTile.Access[lastSide - 1, lane] = true;
            }
        }

        /// <summary>
        /// Adds a vehicle to the tile.
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="v"></param>
        /// <param name="Side"></param>
        /// <param name="lane"></param>
        public void AddVehicle(SimControl sim, Vehicle v, int Side, int lane)
        {
            List<List<Vehicle>> sideVehicles = vehicles[Side - 1];
            List<Vehicle> laneVehicles = sideVehicles[lane];
            laneVehicles.Add(v);
            numberOfVehicles++;
            //returns false if the tile is full
            if (laneVehicles.Count > 5 && this.name != "Spawner" && this.name != "Crossroad" && this.name != "Fork")
            {
                Tile lastTile = sim.simulationMap.GetSurroundingTiles(this.position)[(Side + 1) % 4];
                lastTile.Access[Side - 1, lane] = false;
            }
        }

    }
}
