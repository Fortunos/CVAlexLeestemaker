using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Resources;

namespace TrafficSimulation
{
    public class Simulation
    {
        private SimControl simControl;
        private List<Spawner> spawnerList;

        public delegate void MethodInvoker();
        public Thread thread;
        public bool simStarted;
        protected int waitingCars;
        Boolean simPause;
        protected int simSleep;
        public int extraSpeed;
        public List<string[]> efficiencyNumbers;
        protected static System.Security.Cryptography.RNGCryptoServiceProvider rnd;//om de auto's een willekeurige kant op te laten gaan

        public Simulation(SimControl simControl)
        {
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
            this.simControl = simControl;
            this.simStarted = false;
            spawnerList = new List<Spawner>();
            waitingCars = 0;
            simSleep = 70;
            extraSpeed = 0;
            efficiencyNumbers = new List<string[]>();
            
        }
        public int WaitingCars
        {
            get { return waitingCars; }
            set { waitingCars = value; }
        }
        public int PauseSeconds
        {
            get { return simSleep; }
            set { if (simSleep > 10 && simSleep < 800)simSleep = value; }
        }
        public bool StartSim()
        {
            if (simStarted == false)
            {
                //clearing list for calculating efficiency
                efficiencyNumbers.Clear();
                //spawners verzamelen
                if (Methods.CheckValidConnections(simControl))
                {
                    spawnerList.Clear();
                    simControl.controlList.Clear();

                    foreach (Tile t in simControl.simulationMap.GetMap())
                    {
                        t.Initialize();
                        if (t.name.Equals("Spawner"))
                            spawnerList.Add((Spawner)t);
                    }
                    simControl.MakeTrafficControlList();
                    ThreadStart threadDelegate = new ThreadStart(Update);
                    thread = new Thread(threadDelegate);
                    thread.Start();
                    simStarted = true;
                    return true;
                }
                else
                {
                    simControl.simwindow.windowselect.ShowMessage("niet alle tiles liggen aan elkaar");
                    return false;

                }
            }
            else
            {
                thread.Abort();
                simStarted = false;
                simControl.UpdateInfoBalkSimulatie();
                return false;
            }
        }

        public void StartSimknop()
        {
            if (!simStarted)
                StartSim();
            else
                simPause = false;

        }
        public void PauseSimknop()
        {
            simPause = true;
        }
        public void Update()
        {
            while (true)
            {
                if (!simPause)
                {
                    if (simControl.InvokeRequired)
                        simControl.Invoke(new MethodInvoker(UpdateGame));
                    else
                        UpdateGame();
                }
                //simControl.DrawSelectLine(simControl.selectedTile.position);
                //standaardtijd ingebouwd voor een mooiere weergave.
                Thread.Sleep(simSleep);
            }
        }

        public void UpdateGame()
        {
            //de vehiclemap wordt weer helemaal leeg gemaakt zodat de auto's maar 1 keer getekend worden
            Graphics g = Graphics.FromImage((Image)simControl.vehicleBC.bitmap);
            g.Clear(Color.Transparent);
            Tile[] tiles = simControl.simulationMap.GetMap().ToArray();
            Array.Copy(simControl.simulationMap.GetMap().ToArray(), tiles, simControl.simulationMap.GetMap().Count());
            //alle auto's updaten en weer tekenen
            foreach (Tile t in tiles)
            {
                if (t != null)
                {
                    List<List<Vehicle>> tileVehicles = new List<List<Vehicle>>();
                    foreach (List<List<Vehicle>> list in t.vehicles)
                    {
                        foreach (List<Vehicle> vehiclelist in list)
                            tileVehicles.Add(new List<Vehicle>(vehiclelist));
                    }
                    foreach (List<Vehicle> list in tileVehicles)
                    {
                        foreach (Vehicle v in list)
                        {
                            if(v.Direction == 2 && t.name=="Crossroad")
                            {

                            }
                            UpdateVehicle(t, v);
                            simControl.vehicleBC.AddObject(v.Bitmap, v.position);
                        }
                    }
                }
            }
            //updaten van de trafficlights
            foreach (TrafficlightControl tL in simControl.controlList)
            {
                tL.Run(extraSpeed, (double)simSleep - (double)50);
            }
            //updaten van de spawners
            foreach (Spawner spawn in spawnerList)
            {
                spawn.Tick(simControl, extraSpeed, (double)simSleep - (double)50);
            }
            if (simControl.selectedTile != null)
                simControl.DrawSelectLine(simControl.selectedTile.position);
            simControl.UpdateInfoBalkSimulatie();
            if (simControl.simwindow.InfoBalk.EfWindow != null)
            {
                string date = DateTime.Now.ToString("H:mm:ss");
                string[] efficiencyTime = new string[] { DateTime.Now.ToString("H:mm:ss:fff"), "" + simControl.simwindow.InfoBalk.labelEfficientieNumber.Content };
                efficiencyNumbers.Add(efficiencyTime);
                simControl.simwindow.InfoBalk.EfWindow.t_Tick(null, null);
            }
            simControl.Invalidate();
        }

        private void UpdateVehicle(Tile t, Vehicle v)
        {
            if (v.Speed == 0)
                waitingCars--;
            v.Speed = t.MaxSpeed + extraSpeed;
            //if vehicle has to dissapear ----- moet worden vervangen door zwart vlak over de spawner-----
            if (VehicleIsOnEndSpawner(v, t))
            {
                simControl.simulationMap.GetTileMea(t.position.X, t.position.Y).RemoveVehicle(simControl, v, v.Direction, v.LastDirection, v.Lane);
                simControl.totalCars--;
            }
            if (StaysOnTile(t, v))//if vehicle is still on the tile 
            {
                if (DistanceFromCars(t, v))
                {
                    //if there are other cars standing in front
                    v.Update(t);
                }
                else
                {
                    v.Speed = 0;
                    waitingCars++;
                }
            }
            else
            {
                if (t.Access[v.NextDirection - 1, v.Lane])//if the next tile is accessible
                {
                    //remove vehicle from old tile and add vehicle to new tile
                    Tile[] test = simControl.simulationMap.GetSurroundingTilesSim(t.position);
                    //simControl.simulationMap.GetTile(t.position).RemoveVehicle(simControl, v, v.LastDirection, v.Lane);
                    Tile nextTile = simControl.simulationMap.GetSurroundingTilesSim(t.position)[v.NextDirection - 1];
                    simControl.simulationMap.GetTile(t.position).RemoveVehicle(simControl, v, v.Direction, v.Direction, v.oldLane);
                    v.reset();
                    int oldLane = v.Lane;
                    if (nextTile != null)
                    {
                        v.Speed = nextTile.maxSpeed;
                        v.LastDirection = v.Direction;
                        v.Direction = v.NextDirection;
                        v.oldLane = v.Lane;
                        nextTile.AddVehicle(simControl, v, v.Direction, v.Lane);
                        GetRandomOutDirection(nextTile, v);
                        v.endPosition = GetEndPosition(nextTile, v);
                        v.Update(nextTile);
                    }
                    
                    
                    
                }
                else
                {
                    v.Speed = 0;
                    waitingCars++;
                }
            }
        }

        //returns if an object is still on the tile
        private bool StaysOnTile(Tile t, Vehicle v)
        {
            return CorrectDistance(t, v, 0);
        }

        //returns if there are no other cars standing in front
        private bool DistanceFromCars(Tile t, Vehicle v)
        {
            int distance = 0;//distance between the end of the tile and the last car standing still.
            List<List<Vehicle>> vehicleList = simControl.simulationMap.GetTileMea(t.position.X, t.position.Y).vehicles[v.Direction - 1];
            distance = vehicleList[v.Lane].IndexOf(v) * 16;
            if (t.name == "Fork" || t.name == "Crossroad")
                return true;
            return CorrectDistance(t, v, distance);
        }

        //calculates the places and returns if the vehicle is allowed to drive
        private bool CorrectDistance(Tile t, Vehicle v, int CarSpace)
        {
            switch (v.NextDirection)
            {
                case 1: if (v.position.Y - v.Speed>= t.position.Y+ CarSpace)
                        return true;
                    break;
                case 2: if (v.position.X + v.Speed + v.Bitmap.Width + 5 <= t.position.X + t.size.Width - CarSpace)
                        return true;
                    break;
                case 3: if (v.position.Y + v.Speed + v.Bitmap.Width + 5 <= t.position.Y + t.size.Height - CarSpace)
                        return true;
                    break;
                case 4: if (v.position.X - v.Speed - 5 >= t.position.X + CarSpace)
                        return true;
                    break;
            }
            return false;
        }

        private bool VehicleIsOnEndSpawner(Vehicle v, Tile t)
        {
            if (t.name == "Spawner" && t.Directions.Contains((v.Direction + 1) % 4 + 1))
            {
                switch (v.Direction)
                {
                    case 1: if (v.position.Y - v.Speed <= t.position.Y + 15)
                            return true;
                        break;
                    case 2: if (v.position.X + v.Speed + 15 >= t.position.X + 85)
                            return true;
                        break;
                    case 3: if (v.position.Y + v.Speed + 15 >= t.position.Y + 85)
                            return true;
                        break;
                    case 4: if (v.position.X - v.Speed <= t.position.X + 15)
                            return true;
                        break;
                }
            }
            return false;
        }

        public Point GetEndPosition(Tile tile, Vehicle v)
        {
            int randomLane = 0;
            
            if (tile.name == "Spawner" || tile.name == "Road")
            {
                randomLane = v.Lane;
                //v.Lane = randomLane;
            }
            else
            {
                Tile endTile = simControl.simulationMap.GetConnectingTiles(tile.position)[v.NextDirection - 1];
                int tileLanes = endTile.GetLanesIn((v.NextDirection + 1) % 4 + 1);
                randomLane = Math.Abs(Guid.NewGuid().GetHashCode()) % tileLanes;
                v.Lane = randomLane;
            }



            switch(v.NextDirection)
            {
                case 1:
                    return new Point(tile.position.X + 53 + (randomLane * 16), tile.position.Y );
                case 2:
                    if (v.LastDirection == 1 && v.Direction != 2)
                        return new Point(tile.position.X - v.Bitmap.Height + 100 - 5, tile.position.Y + 53 + (16 * randomLane) - v.Bitmap.Height);
                    else
                        return new Point(tile.position.X - v.Bitmap.Height + 100 - 5, tile.position.Y + 53 + (16 * randomLane));
                case 3:
                    if(v.LastDirection == 4)
                    return new Point(tile.position.X + 37 - (16 * randomLane), tile.position.Y + 100 - v.Bitmap.Height - 5);
                    else
                        return new Point(tile.position.X + 37 - (16 * randomLane), tile.position.Y + 100 - v.Bitmap.Width - 5);
                case 4 :
                    return new Point(tile.position.X+5,tile.position.Y+ 37 -(16*randomLane));
                default :
                    return new Point(0,0);
            }
        }

        private void GetRandomOutDirection(Tile tile, Vehicle v)
        {
            int newDirection = 0;
            switch (tile.name)
            {
                case "Spawner":
                    newDirection = v.Direction;
                    break;
                case "Road":
                    foreach (int i in tile.Directions)
                    {
                        if (i != ((v.Direction + 1) % 4)+1)
                        {
                            newDirection = i;
                        }
                    }
                    break;
                case  "Crossroad" :
                    switch (tile.GetLanesIn((v.Direction+1)%4+1))
                    {
                        case 1: while (newDirection == 0 || newDirection == (v.Direction + 1) % 4 + 1)
                                    newDirection = RandomNumber(4);
                            break;
                        case 2:
                            
                                switch(v.Lane)
                                {
                                    
                                case 1: int number = RandomNumber(2);
                                    if (number == 1)
                                        newDirection = (v.Direction) % 4 + 1;
                                    else
                                        newDirection = v.Direction;
                                    break;
                                case 0: newDirection = (v.Direction+2) % 4 + 1;
                                    break;
                            }
                            break;
                        case 3:
                            switch (v.Lane)
                            {
                                case 0: newDirection = (v.Direction + 2) % 4 + 1;
                                    break;
                                case 1: newDirection = v.Direction;  
                                    break;
                                case 2: newDirection = (v.Direction) % 4 + 1;
                                    break;
                            }
                            break;
                    }
                    break;
                case "Fork" :
                    switch (tile.GetLanesIn((v.Direction + 1) % 4 + 1))
                        {
                            case 1: while (newDirection == 0 || newDirection == (v.Direction + 1) % 4 + 1 || newDirection == tile.NotDirection)
                                    newDirection = RandomNumber(4);
                                break;
                            case 2:
                                if (v.Direction == (tile.NotDirection + 2) % 4 + 1)
                                {
                                    switch (v.Lane)
                                    {
                                        case 1: newDirection = v.Direction;
                                            break;
                                        case 0: newDirection = (v.Direction + 2) % 4 + 1;
                                            break;
                                    }
                                }
                                else if(v.Direction == (tile.NotDirection) % 4 + 1)
                                {
                                     switch (v.Lane)
                                    {
                                        case 1:  newDirection = (v.Direction) % 4 + 1;
                                            break;
                                        case 0: newDirection = v.Direction;
                                            break;
                                    }   
                                }
                                else
                                {   
                                    switch (v.Lane)
                                    {
                                        case 1: newDirection = (v.Direction) % 4 + 1;
                                            break;
                                        case 0: newDirection = (v.Direction + 2) % 4 + 1;
                                            break;
                                    }
                                }
                            break;

                            case 3:
                                if ((v.Direction + 1) % 4 + 1 == (tile.NotDirection + 2) % 4 + 1)
                                {
                                    switch (v.Lane)
                                    {
                                        case 2:  newDirection = (v.Direction) % 4 + 1;
                                            break;
                                        case 1: newDirection = v.Direction;
                                            break;
                                        case 0: newDirection = v.Direction;
                                            break;
                                    }
                                }
                                else if ((v.Direction + 1) % 4 + 1 == (tile.NotDirection) % 4 + 1)
                                {
                                    switch (v.Lane)
                                    {
                                        case 2: newDirection = v.Direction;
                                            break;
                                        case 1: newDirection = v.Direction;
                                            break;
                                        case 0: newDirection = (v.Direction + 2) % 4 + 1;
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (v.Lane)
                                    {
                                        case 2: newDirection = (v.Direction) % 4 + 1;
                                            break;
                                        case 1: newDirection = (v.Direction) % 4 + 1;
                                            break;
                                        case 0: newDirection = (v.Direction + 2) % 4 + 1;
                                            break;
                                    }
                                }
                                break;
                        }
                    break;
            }
            //if (v.UpdatePoint == 0)
            {
                if (v.Direction - newDirection < 0)
                {
                    v.Bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
                v.NextDirection = newDirection;
            }
        }

        private int RandomNumber(int Max)
        {
            Byte[] random;
            random = new Byte[1];
            rnd.GetBytes(random);
            return random[0] % Max+1;
        }
    }
}