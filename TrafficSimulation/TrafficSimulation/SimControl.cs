using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Integration;


namespace TrafficSimulation
{
    public partial class SimControl : UserControl
    {
        public SimWindow simwindow;
        //BitmapControls used for the bitmaps in which the background, vehicles and trafficlights are stored
        public BitmapControl backgroundBC, trafficlightBC, vehicleBC;
        //PictureBox 
        public PictureBox backgroundPB, trafficlightPB, vehiclePB;
        //
        Point mouseDownPoint;
        Point mouseMovePoint;
        public SimulationMap simulationMap;
        //the old selected tile
        public Tile oldselectedTile;
        //tile which is selected for the infobalk
        public Tile selectedTile;
        //max tiles fitting horizontal on the map
        public int tilesHorizontal;
        //the simulation, has a new thread which is started when the simulation starts
        public Simulation simulation;
        //list for all the trafficlight controls 
        public List<TrafficlightControl> controlList = new List<TrafficlightControl>();
        //selected tile for building
        public Tile currentBuildTile;
        //variable for click methods: state indicates which button is clicked and which action follows
        public String state = "selected";
        //if map is moved
        bool isMoved;
        //number for gamespeed for infobalk
        public double gameSpeed;
        //counter for all trafficlights for in the infobalk
        public int AmountOfTrafficlights;
        //counter of tiles for in the infobalk
        public int AmountOfTiles;
        //counter for cars for in infobalk
        public int totalCars;

        public SimControl(Size size, SimWindow simwindow)
        {
            this.simwindow = simwindow;
            simulationMap = new SimulationMap(this);
            //methode in the partial class creating all the objects needed for the simulation
			this.Size = new Size(2000, 1500); //has to be changed to the windowsize
            Point bitmapLocation = new Point(-((this.Size.Width - Screen.PrimaryScreen.Bounds.Width) / 2), -((this.Size.Height - Screen.PrimaryScreen.Bounds.Height) / 2));
            this.Location = bitmapLocation;
            this.BackColor = Color.Green;
            /* 
             * De bitmapControls in which the simulation takes place, all the bitmapcontrols have a bitmap with
             * which it interacts. Use the BitmapControl to change the bitmaps used for the simulation
             */
            backgroundBC = new BitmapControl(this.Size);
            trafficlightBC = new BitmapControl(this.Size);
            vehicleBC = new BitmapControl(this.Size);

            totalCars = 0;
            //
            isMoved = false;
            //
            mouseDownPoint = new Point(0, 0);
            mouseMovePoint = new Point(0, 0);
            //amount of tiles that fit in the bitmap horizontally
            tilesHorizontal = Size.Width / 100;
            this.Visible = true;
            gameSpeed = 1;
            this.simulation = new Simulation(this);
            InitializeComponent();
            //The simulation thread will be started here, the whole simulation will be regulated in this class.
            this.simulation = new Simulation(this);
            vehicleBC.AddGrid();
        }

        /// <summary>
		/// Draws a blue line surrounding the selected tile
        /// </summary>
        /// <param name="mea">location of the mouse</param>
        public void DrawSelectLine(Point mea)
        {
			/// If the point has a road, draw blue line
            if (simulationMap.GetTileMea(mea.X, mea.Y) != null)
            {
                Tile selectTile = new SelectTile();

                /// Drawing blue line
                selectTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100));
                selectTile.DrawImage();

                /// The current selectedTile becomes the oldselectedTile
                oldselectedTile = simulationMap.GetTileMea(mea.X, mea.Y);
                selectedTile = simulationMap.GetTileMea(mea.X, mea.Y);
                UpdateInfoBalkDesign();
                simwindow.BovenSchermRechts.ShowOrHideInfoBalk(true);

            }
			/// If the point doesn't have a road
            else
            {
				/// Remove blue line
                if (selectedTile != null)
                    backgroundBC.AddObject(selectedTile.DrawImage(), selectedTile.position);

				/// Set selectedTile and oldSelectedTile to null
                oldselectedTile = null;
                selectedTile = null;

				/// Hide info screen
                simwindow.BovenSchermRechts.ShowOrHideInfoBalk(false);
            }

            this.Invalidate();
        }

        /// <summary>
        /// Removes a tile
        /// </summary>
        /// <param name="mea">location of the mouse</param>
        public void removeTile(Point mea)
        {
            if (simulationMap.GetTileMea(mea.X, mea.Y) != null)
            {
                simwindow.BovenSchermRechts.ShowOrHideInfoBalk(false);
                Bitmap tileImage;
                Tile selectedTile = new removeTile();
                //selectedTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100));
                tileImage = selectedTile.DrawImage();
                backgroundBC.AddObject(tileImage, simulationMap.GetPosition(new Point(mea.X, mea.Y)));
                trafficlightBC.AddObject(tileImage, simulationMap.GetPosition(new Point(mea.X, mea.Y)));
                AmountOfTiles--;
                if (simulationMap.GetTileMea(mea.X, mea.Y).name == "Crossroad" || simulationMap.GetTileMea(mea.X, mea.Y).name == "Fork")
                {

                    AmountOfTrafficlights--;
                    UpdateInfoBalkDesign();
                }
                simulationMap.RemoveTile(simulationMap.GetTileMea(mea.X, mea.Y));
                trafficlightBC.bitmap.MakeTransparent(Color.Green);
                UpdateInfoBalkDesign();
                this.Invalidate();
            }
        }

		/// <summary>
		/// Draw a tile
		/// </summary>
		/// <param name="mea">location of the mouse</param>
		/// <param name="buildTile">tile that has to be drawn</param>
        public void DrawTile(Point mea, Tile buildTile)
        {
            Bitmap tileImage;

            if (Methods.TileConnectionisValid(this, buildTile, mea))
            {
                simwindow.BovenSchermRechts.ShowOrHideInfoBalk(false);
                removeTile(mea);

                AmountOfTiles++;
                //tile is placced in list
                simulationMap.AddTile(buildTile);
                buildTile.SetValues(this, simulationMap.GetPosition(new Point(mea.X, mea.Y)));
                tileImage = buildTile.DrawImage();
                //Map is updated with the new tile
                backgroundBC.AddObject(tileImage, simulationMap.GetPosition(new Point(mea.X, mea.Y)));
                selectedTile = buildTile;
                trafficlightBC.bitmap.MakeTransparent(Color.Green);
				//A new buildTile is created with the same values as before because a new tile can than be clicked in
                currentBuildTile = Methods.CopyCurrentTile(this, buildTile);
                if (buildTile.name == "Crossroad" || buildTile.name == "Fork")
                {
                    this.AmountOfTrafficlights++;
                }
                UpdateInfoBalkDesign();
                this.Invalidate();
            }
        }

        /// <summary>
		/// Moves the map if is dragged
        /// </summary>
        /// <param name="mea">location of mouse</param>
        private void MoveMap(MouseEventArgs mea)
        {
            if (Math.Abs(mea.X - mouseMovePoint.X) > 3 || Math.Abs(mea.Y - mouseMovePoint.Y) > 3)
            {
                Point newPosition = new Point(backgroundPB.Location.X + (mea.X - mouseMovePoint.X), backgroundPB.Location.Y + (mea.Y - mouseMovePoint.Y));
                Rectangle trafficGround = new Rectangle(trafficlightPB.Location, trafficlightPB.Size);
                if (trafficGround.Contains(mea.Location))
                {
                    backgroundPB.Location = newPosition;
                    isMoved = true;
                }
                this.Update();
            }
        }

        /// <summary>
        /// Remove tile from the map.
        /// </summary>
        public void ClearRoad()
        {
            foreach (Tile t in simulationMap.GetMap())
            {
                foreach (List<List<Vehicle>> list in t.vehicles)
                {
                    foreach (List<Vehicle> l in list)
                    {
                        l.Clear();
                    }
                }
            }
            //Removes all cars
            Graphics g = Graphics.FromImage((System.Drawing.Image)vehicleBC.bitmap);
            g.Clear(System.Drawing.Color.Transparent);
            //reset speed values
            simulation.PauseSeconds = 50;
            simulation.extraSpeed = 0;
            backgroundPB.Invalidate();
        }

		/// <summary>
		/// Create a list of all the trafficlightcontrols
		/// </summary>
        public void MakeTrafficControlList()
        {
            int tempTrafficlightCount = 0;
            foreach (Tile t in simulationMap.GetMap())
            {
                if (t.name == "Crossroad")
                {
                    Crossroad Cr = (Crossroad)t;
                    if (Cr.control != null)
                    {
                        tempTrafficlightCount++;
                        controlList.Add(Cr.control);
                    }
                }
                if (t.name == "Fork")
                {
                    Fork f = (Fork)t;
                    if (f.control != null)
                    {
                        tempTrafficlightCount++;
                        controlList.Add(f.control);
                    }
                }
            }
            this.AmountOfTrafficlights = tempTrafficlightCount;
        }

		/// <summary>
		/// Updates the infoscreen when in design mode
		/// </summary>
        public void UpdateInfoBalkDesign()
        {
            if (selectedTile != null)
            {
                int trafficStrategy = 0;
                if (selectedTile.name == "Fork")
                {
                    Fork tile = (Fork)selectedTile;
                    trafficStrategy = tile.control.strat;
                }
                else if (selectedTile.name == "Crossroad")
                {
                    Crossroad tile = (Crossroad)selectedTile;
                    trafficStrategy = tile.control.strat;
                }
                int[,] lanes = new int[4, 2];
                for (int i = 0; i < 4; i++)
                {
                    lanes[i, 1] = selectedTile.GetLanesIn(i + 1);
                    lanes[i, 0] = selectedTile.GetLanesOut(i + 1);
                }
                simwindow.InfoBalk.UpdateDesign(lanes, selectedTile.maxSpeed, AmountOfTiles, AmountOfTrafficlights, trafficStrategy, gameSpeed);
            }
        }

		/// <summary>
		/// Update the infoscreen when in simulation mode
		/// </summary>
        internal void UpdateInfoBalkSimulatie()
        {
            int vehicleNumber = 0;
            if (selectedTile != null)
                vehicleNumber = selectedTile.NumberOfVehicles;
            simwindow.InfoBalk.UpdateSimulation(totalCars, simulation.WaitingCars, vehicleNumber, gameSpeed);
        }

		/// <summary>
		/// Reset some counters used in the simulation
		/// </summary>
        public void ResetSimulationCounters()
        {
            this.totalCars = 0;
            simulation.WaitingCars = 0;
            gameSpeed = 1;
            simulationMap.ResetCarsOnTile();
        }
    }
}
