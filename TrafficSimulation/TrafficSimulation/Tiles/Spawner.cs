using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Spawner : Tile
    {
        /*
         * Variables of Spawner
         *      lanesOut is the number of lanes going out of the tile
         *      LanesIn is the number of lanes going into the tile
         *      
         *      carsSpawnChance is the chance to spawn a new vehicle, the higher this number is the higher chance of spawning a new vehicle
         *      spawnLane lane on which the vehicle is spawned
         *      currentSpawn is used to a part of the car, this number is decremented each time it's higher than one
         *      spawnPerTick, currentspawn is increased by this number evrey gametick.
         */
        //spawner variables
        private int lanesOut;
        private int lanesIn;
        //variables used to spawn vehicles
        private int carsSpawnChance;
        private int spawnLane;
        private double currentSpawn;
        private double spawnPerTick;
        //random number generator
        protected static System.Security.Cryptography.RNGCryptoServiceProvider rnd;

        /// <summary>
        /// Constructor used by fork, based on the constructor in Tile
        /// here all default values of the Spawner are set.
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="direction"></param>
        public Spawner(SimControl sim, int direction)
        {
            this.name = "Spawner";
            carsSpawnChance = 3;
            spawnLane = 0;
            this.lanesIn = 1;
            this.lanesOut = 1;
            this.spawnPerTick = 0.05;
            directions.Add(direction);
            currentSpawn = 1;
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();


        }
        /*
         * getters and setters used for spawning vehicles
         */
        public double CurrentSpawn { get { return currentSpawn; } }
        public int SpawnLane { get { return spawnLane; } }
        public int CarsSpawnChance { get { return carsSpawnChance; } set { carsSpawnChance = value; } }

        /// <summary>
        /// Based on the method GetLanesIn in Spawner
        /// returns the lanes going into the tile in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesIn(int direction)
        {
            if (this.Direction == direction)
                return lanesIn;
            else if (this.Direction == (direction + 1) % 4 + 1)
                return lanesOut;
            else
                return 1;
        }

        /// <summary>
        /// Based on the method SetValues in Tile
        /// sets the values given to the Spawner when the Spawner is placed on the map.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);
            DrawSpawnerBlock(s);
        }

        /// <summary>
        /// Based on the method GetLanesOut in Spawner
        /// returns the lanes going out of the tile in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesOut(int direction)
        {
            if (this.Direction == direction)
                return lanesOut;
            else if (this.Direction == (direction + 1) % 4 + 1)
                return lanesIn;
            else
                return 1;
        }

        /// <summary>
        /// Based on the method GetControl in Tile
        /// returns the TrafficLightControl used by the Spawner.
        /// </summary>
        /// <returns></returns>
        public override TrafficlightControl GetControl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Based on the method UpdateLanes in Spawner
        /// this method is called when the lanes are updated.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="direction"></param>
        /// <param name="lanesIn"></param>
        /// <param name="lanesOut"></param>
        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (direction == this.Direction)
            {
                this.lanesIn = lanesIn;
                this.lanesOut = lanesOut;
                DrawSpawnerBlock(s);
            }
        }

        private void DrawSpawnerBlock(SimControl s)
        {
            for (int i = this.position.X; i < this.position.X + 100; i++)
                for (int j = this.position.Y; j < this.position.Y + 100; j++)
                    s.trafficlightBC.bitmap.SetPixel(i, j, Color.Transparent);
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSpawnerBlock(Graphics.FromImage(image), Direction, lanesOut, lanesIn);
            s.trafficlightBC.AddObject(image, this.position);
        }
        /// <summary>
        /// method called on each gametick.
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="extraSpeed"></param>
        /// <param name="extraTime"></param>
        public void Tick(SimControl sim, int extraSpeed, double extraTime)
        {
            currentSpawn += (spawnPerTick * (extraSpeed + 1)) + ((extraTime / 50) * spawnPerTick);

            if (currentSpawn >= 1)
            {
                Spawn(sim);
            }
        }

        /// <summary>
        /// spawns a new car with a randomiser to make the spawns come at random moments
        /// </summary>
        /// <param name="sim"></param>
        public void Spawn(SimControl sim)
        {
            Byte[] random;
            random = new Byte[1];
            rnd.GetBytes(random);
            if (random[0] % carsSpawnChance == 0)
            {
                spawnLane = ((random[0] * 10) / 8) % lanesOut;
                List<List<Vehicle>> vehicleList = vehicles[this.Direction - 1];
                if (vehicleList[spawnLane].Count < 4)
                {
                    Vehicle auto = createVehicle(spawnLane);
                    auto.endPosition = sim.simulation.GetEndPosition(this, auto);
                    AddVehicle(sim, auto, Direction, spawnLane);
                    sim.totalCars++;
                }
            }
            currentSpawn--;
        }

        /// <summary>
        /// returns a new vehicle with default values
        /// </summary>
        /// <param name="spawnLane"></param>
        /// <returns></returns>
        public Vehicle createVehicle(int spawnLane)
        {
            return new NormalCar(this.position, this.position, 10, this.maxSpeed, this.Direction, spawnLane);
        }

        /// <summary>
        /// Based on the method doesConnect in Tile
        /// returns true if the given side connects to another tile and false if it doesn't.
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public override bool doesConnect(int side)
        {
            if ((side + 1) % 4 + 1 == this.Direction)
                return true;
            return false;
        }

        /// <summary>
        /// Based on the method DrawImage in Tile
        /// returns the bitmap of the Spawner.
        /// </summary>
        /// <returns></returns>
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSpawner(Graphics.FromImage(image), Direction, lanesOut, lanesIn);
            return image;
        }
    }
}
