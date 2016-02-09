using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrafficSimulation
{
    /// <summary>
    /// Interaction logic for BovenScherm.xaml
    /// </summary>
    public partial class BovenSchermRechts : UserControl
    {
        public Boolean InfoVisible = false;
        //private bool simulationStarted = false;
        InfoBalk infoBalk;
        OnderScherm onderScherm;
        WindowSelect windowselect;
        int breedteScherm, breedteInfoBalk, hoogteBovenBalk;

        public BovenSchermRechts(WindowSelect ws, InfoBalk info, OnderScherm Onder, int bs, int bib, int hbb)
        {
            windowselect = ws;
            infoBalk = info;
            onderScherm = Onder;
            breedteScherm = bs;
            breedteInfoBalk = bib;
            hoogteBovenBalk = hbb;
            InitializeComponent();
        }

		/// <summary>
		/// Opens and closes the information thing.
		/// It's by default hidden.
		/// </summary>
        public void Info_Click(object sender, RoutedEventArgs e)
        {
            if (InfoVisible)
                ShowOrHideInfoBalk(false);
            else
                ShowOrHideInfoBalk(true);
        }

        public void ShowOrHideInfoBalk(Boolean infoVisible)
        {
            this.InfoVisible = infoVisible;
            /// Hide info
            if (!InfoVisible)
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
                InfoVisible = false;

            }
            // Show info
            else
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point((breedteScherm - breedteInfoBalk), hoogteBovenBalk);
                InfoVisible = true;
            }
        }

		/// <summary>
		/// Go back to the homescreen.
		/// </summary>
		public void Home_Click(object sender, RoutedEventArgs e)
        {
            if (windowselect.simwindow.simcontrol.simulation.simStarted == true)
            {
				windowselect.simwindow.BovenSchermLinks.SimulationDesign_Click(null, null);
                windowselect.simwindow.simcontrol.simulation.simStarted = false;
            }

			windowselect.Start();
        }

		/// <summary>
		/// Method for saving all the tiles
		/// </summary>
		public void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// New savedialog
				System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

				/// Custom filename
				int number = 1;
				string fileName = "Traffic" + number.ToString();

				
				/// Set some information
				saveDialog.DefaultExt = ".trs"; // Extension name
				saveDialog.FilterIndex = 1;		// Fiter index in save form
				saveDialog.Filter = "Traffic Simulation Files (*.trs) | *.trs";	// Filter for files
				saveDialog.RestoreDirectory = true;	// Saves the directory you're in
				saveDialog.FileName = fileName;		// Set filename
				saveDialog.OverwritePrompt = true;	// Set the ability to overwrite another file to true

				/// Is the button "Save" pressed?
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					/// Change cursor to wait cursor
					this.Cursor = Cursors.Wait;

					/// New file
					StreamWriter file = new StreamWriter(@saveDialog.FileName);

					/// File can be bigger than 1024
					file.AutoFlush = true;

					/// Remove all duplicates
					List<Tile> list = windowselect.simwindow.simcontrol.simulationMap.GetMap().Distinct().ToList();

					/// Get every tile in the list
					foreach (Tile tile in list)
					{
						/// If the tile has some value asigned to it						
						if (tile != null)
						{
							string currenttile = tile.name;

							// TODO: Extra save options

							/// Every tile has his own information
							/// For saving them, you need the specific information for each tile
							/// This is done with multiple cases, on for each tile
							/// 
							/// Basic information
							///		 0: Tile
							///		 1: X position
							///		 2: Y position
							///	Specific information
							///		 3: Trafficlight strat
							///		 4: Maxspeed for a tile
							///		 5: Begin direction (notDirection for Fork, direction for Spawner)
							///		 6: End direction (only for Road)
							///		 7: laneshightolow, not for Crossroad and Fork
							///		 8: laneslowtohigh, not for Crossroad and Fork
							///		 9: Cars per second in spawner

							switch (currenttile)
							{
								// Save case for a fork
								case "Fork":
									file.WriteLine(
										tile.name + "_" +					// 0 Welke tile
										tile.position.X + "_" +				// 1 X positie
										tile.position.Y + "_" +				// 2 Y positie
										tile.GetControl().strat + "_" + 	// 3 strat
										tile.maxSpeed + "_" +				// 4 Maxspeed
										tile.NotDirection);					// 5 De not direction
									break;

								// Save case for a crossroad
								case "Crossroad":
									file.WriteLine(
										tile.name + "_" +					// 0 Welke tile
										tile.position.X + "_" +				// 1 X positie
										tile.position.Y + "_" +				// 2 Y positie
										tile.GetControl().strat + "_" + 	// 3 strat
										tile.maxSpeed);						// 4 Maxspeed
									break;

								// Save case for a road (that is a straight road or a curved road)
								case "Road":
									file.WriteLine(
										tile.name + "_" +			// 0 Welke tile
										tile.position.X + "_" +		// 1 X positie
										tile.position.Y + "_" +		// 2 Y positie
										" " + "_" +					// 3 Empty
										tile.maxSpeed + "_"	+		// 4 Maxpeed
										tile.StartDirection + "_" +	// 5 Begin richting
										tile.EndDirection + "_" +	// 6 Eind richting
										tile.LanesHighToLow + "_" + // 7 Wegen hoog, laag
										tile.LanesLowToHigh);		// 8 Wegen laag, hoog
									break;

								// Save case for a spawner
								case "Spawner":
									Spawner spawner = (Spawner)tile;

									file.WriteLine(
										tile.name + "_" +			// 0 Welke tile
										tile.position.X + "_" +		// 1 X positie
										tile.position.Y + "_" +		// 2 Y positie
										" " + "_" +					// 3 Empty
										tile.maxSpeed + "_" +		// 4 Maxspeed
										tile.Direction + "_" +		// 5 Richting
										" " + "_" +					// 6 Empty
										tile.GetLanesOut((tile.Direction + 1) % 4 + 1) + "_" +	// 7 LanesHighToLow
										tile.GetLanesIn((tile.Direction + 1) % 4 + 1) + "_" +	// 8 LanesLowToHigh
										spawner.CarsSpawnChance);								// 9 Spawn speed
									break;
							}
						}
					}

					/// Change cursor back to arrow
					this.Cursor = Cursors.Arrow;
				}
			}
			/// Throw an exception
			catch (Exception exp)
			{
				/// Change cursor back to arrow
				this.Cursor = Cursors.Arrow;

				/// Show exception and the error message
				MessageBox.Show("Error: Could not write file to disk. Original error:" + exp);

            }
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
			///
        }
    }
}
