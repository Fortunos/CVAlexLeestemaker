using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TrafficSimulation
{
    public partial class StartWindow : UserControl
    {
        public WindowSelect windowselect;

		ElementHost StartHost;
		AboutWindow about;
		
        InterfaceStart interfaceStart;
		
        int widthStartScreen, heightStartScreen;

        public StartWindow(Size size, WindowSelect sim)
        {
            this.Size = size;
            widthStartScreen = size.Width;
            heightStartScreen = size.Height;
            windowselect = sim;

            interfaceStart = new InterfaceStart(this, widthStartScreen, heightStartScreen);

            StartHost = new ElementHost()
            {
                Height = size.Height,
                Width = size.Width,
                Location = new Point(0,0),
                Child = interfaceStart,
            };

            /// Add startbuttons
            this.Controls.Add(StartHost);
            StartHost.Left = (this.Size.Width - StartHost.Size.Width) / 2;

        }

        /// <summary>
        /// Click Resume in homescreen
        /// </summary>
        public void Resume_Click()
        {
            /// Set current build tile to a straight road
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 3);

            /// Set state to building mode
            windowselect.simwindow.simcontrol.state = "selected";

            /// Open simcontrol
            windowselect.New();
        }

		/// <summary>
		/// Click New in homescreen
		/// Create an empty field to create a map
		/// </summary>
		internal void New_Click()
		{
			windowselect.simwindow = new SimWindow(windowselect.Size, windowselect);
			Resume_Click();
		}

		/// <summary>
		/// Click Open in homescreen
		/// Loads previous saved files
		/// </summary>
		public void Open_Click()
		{
            Tile[] tempTileList = new Tile[(windowselect.simwindow.simcontrol.Size.Width / 100) * (windowselect.simwindow.simcontrol.Size.Height / 100)];
			/// Does this work?
            SimControl simcontrol;
            
			simcontrol = windowselect.simwindow.simcontrol;
            simcontrol.oldselectedTile = null;

			/// Make new stream
			Stream myStream1 = null;
			Stream myStream2 = null;
			Stream dummyStream = null;

			int addLines = 0;
			int totalLines;

			/// New open dialog
			OpenFileDialog openDialog = new OpenFileDialog();

			/// Few basic settings for the opendialog
			//openFileDialog1.InitialDirectory = "c:\\";
			openDialog.Filter = "TrafficSimulation files (*.trs)|*.trs";
			openDialog.FilterIndex = 1;
			openDialog.RestoreDirectory = true;

			if (openDialog.ShowDialog() == DialogResult.OK)
			{
				/// Change cursor to wait cursor
				this.Cursor = Cursors.WaitCursor;

				/// Make new loadwindow with the startposition in the middle
				LoadWindow LoadWin = new LoadWindow();
				LoadWin.StartPosition = FormStartPosition.CenterScreen;
				LoadWin.FormBorderStyle = FormBorderStyle.FixedDialog;
				LoadWin.ControlBox = false;

				/// Try to open the file
				try
				{
					if (openDialog.OpenFile() != null)
					{
						/// Make 3 streams, 1 for the road and spawners, 2 for the forks and crossroads and the dummy for counting
						myStream1 = openDialog.OpenFile();
						myStream2 = openDialog.OpenFile();
						dummyStream = openDialog.OpenFile();

						/// Streamreaders for each stream
						StreamReader dummyReader = new StreamReader(dummyStream);
						StreamReader r1, r2;

						/// Set the number of lines of code that has to be deleted and added
						addLines = 0;

						/// Count the numbers of lines in the file
						string line;
						while ((line = dummyReader.ReadLine()) != null)
						{
							addLines++;
						}

						/// Total amount of lines that has to be run through while loading
						totalLines = addLines * 2;

						/// Set maximums of the progressbar
						LoadWin.progressBar1.Maximum = totalLines;

						/// Show the LoadWindow
						LoadWin.Show();

						/// Clear the map
						simcontrol.backgroundBC.ClearBitmap();
						simcontrol.backgroundBC.bitmap.MakeTransparent(Color.Green);
						simcontrol.trafficlightBC.ClearBitmap();
						simcontrol.trafficlightBC.bitmap.MakeTransparent(Color.Green);
						simcontrol.Invalidate();

						/// Clear the list
						simcontrol.simulationMap.ClearTileList();

						#region Add roads and spawners to the map

						/// Add al the roads to the map
						using (myStream1)
						{
							// Streamreader for stream 1
							r1 = new StreamReader(myStream1);

							// Set value of progressBar1 back to 0 and change maximum
							LoadWin.progressBar1.Value = 0;
							LoadWin.progressBar1.Maximum = (addLines * 2);


							/// Read the file line for line
							while (r1.Peek() >= 0)
							{
								string t = r1.ReadLine();

								// Char that splits the data
								char[] splitChar = { '_' };

								// Array of strings with info about tile
								string[] information = t.Split(splitChar);

								Bitmap tileImage;
								Tile currentBuildTile;
								int roadX;
								int roadY;

								int tilesHorizontal = Size.Width / 100;

								/// You need different information from different tiles
								/// So you need multiple cases, one for each tile
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
								
								switch (information[0])
								{
									case "Road":
										/// Make new tile
										currentBuildTile = new Road(Convert.ToInt32(information[5]), Convert.ToInt32(information[6]));

										/// Get the location
										roadX = Convert.ToInt32(information[1]) / 100;
										roadY = Convert.ToInt32(information[2]) / 100;

										/// Set some values
										currentBuildTile.SetValues(simcontrol, new Point((roadX * 100), roadY * 100));
										currentBuildTile.LanesHighToLow = Convert.ToInt32(information[7]);
										currentBuildTile.LanesLowToHigh = Convert.ToInt32(information[8]);
										currentBuildTile.maxSpeed = Convert.ToInt32(information[4]);

										/// Add to list
										tempTileList[(roadX + roadY * 20)] = currentBuildTile;

										/// Draw the tile
										tileImage = currentBuildTile.DrawImage();
										simcontrol.backgroundBC.AddObject(tileImage, new Point(roadX * 100, roadY * 100));

										/// Add 1 to progressBar1/2
										LoadWin.progressBar1.PerformStep();

										break;

									/// Load a spawner to the list
									case "Spawner":
										/// Make new tile
                                        currentBuildTile = new Spawner(windowselect.simwindow.simcontrol, Convert.ToInt32(information[5]));
										Spawner spawner = (Spawner)currentBuildTile;

										/// Get location
										roadX = Convert.ToInt32(information[1]) / 100;
										roadY = Convert.ToInt32(information[2]) / 100;

										/// Set some values
										spawner.maxSpeed = Convert.ToInt32(information[4]);
										spawner.UpdateLanes(simcontrol, Convert.ToInt32(information[5]), Convert.ToInt32(information[7]), Convert.ToInt32(information[8]));
										spawner.SetValues(simcontrol, new Point((roadX * 100), roadY * 100));
										spawner.CarsSpawnChance = Convert.ToInt32(information[9]);

										/// Add to list
										tempTileList[(roadX + roadY * 20)] = currentBuildTile;

										/// Draw the tile
										tileImage = currentBuildTile.DrawImage();
										simcontrol.backgroundBC.AddObject(tileImage, new Point(roadX * 100, roadY * 100));
										simcontrol.Invalidate();

										/// Add 1 to progressBar1/2
										LoadWin.progressBar1.PerformStep();

										break;

									default:
										break;
								}
							}

							/// Copy the tempTileList to tileList
                            foreach (Tile tile in tempTileList)
                            {
                                if (tile != null)
                                {
                                    windowselect.simwindow.simcontrol.simulationMap.AddTile(tile);
                                }
                            }
						}

						#endregion

						#region Add the rest to the map

						using (myStream2)
						{
							r2 = new StreamReader(myStream2);

                            tempTileList = new Tile[(windowselect.simwindow.simcontrol.Size.Width / 100) * (windowselect.simwindow.simcontrol.Size.Height / 100)];
							/// Read the file line for line
							while (r2.Peek() >= 0)
							{
								string t = r2.ReadLine();

								// Char that splits the string
								char[] splitChar = { '_' };

								// Array of strings with info about tile
								string[] information = t.Split(splitChar);

								Bitmap tileImage;
								Tile currentBuildTile;
								int roadX;
								int roadY;

								int tilesHorizontal = Size.Width / 100;

								/// You need different information from different tiles
								/// So you need multiple cases, one for each tile
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

								switch (information[0])
								{
									/// Load a fork into the list
									case "Fork":
										/// Make new tile
										currentBuildTile = new Fork(simcontrol, Convert.ToInt32(information[5]));

										/// Get the location
										roadX = Convert.ToInt32(information[1]) / 100;
										roadY = Convert.ToInt32(information[2]) / 100;

										/// Set some values
										currentBuildTile.SetValues(simcontrol, new Point((roadX * 100), roadY * 100));
										currentBuildTile.maxSpeed = Convert.ToInt32(information[4]);
										currentBuildTile.GetControl().strat = Convert.ToInt32(information[3]);

										/// Add to list
										tempTileList[(roadX + roadY * 20)] = currentBuildTile;

										/// Draw the tile
										tileImage = currentBuildTile.DrawImage();
										simcontrol.backgroundBC.AddObject(tileImage, new Point(roadX * 100, roadY * 100));
										simcontrol.Invalidate();

										/// Add 1 to progressBar1/2
										LoadWin.progressBar1.PerformStep();

										break;

									/// Load a crossroad to the list
									case "Crossroad":
										/// Make new tile
										currentBuildTile = new Crossroad(simcontrol);

										/// Get location
										roadX = Convert.ToInt32(information[1]) / 100;
										roadY = Convert.ToInt32(information[2]) / 100;

										/// Set some values
										currentBuildTile.SetValues(simcontrol, new Point((roadX * 100), roadY * 100));
										currentBuildTile.maxSpeed = Convert.ToInt32(information[4]);
										currentBuildTile.GetControl().strat = Convert.ToInt32(information[3]);

										/// Add to list
										tempTileList[(roadX + roadY * 20)] = currentBuildTile;

										/// Draw the tile
										tileImage = currentBuildTile.DrawImage();
										simcontrol.backgroundBC.AddObject(tileImage, new Point(roadX * 100, roadY * 100));
										simcontrol.Invalidate();

										/// Add 1 to progressBar1/2
										LoadWin.progressBar1.PerformStep();

										break;

									default:
										break;
								}
							}

							/// Copy the tempTileList to tileList
							foreach (Tile tile in tempTileList)
							{
								if (tile != null)
								{
									windowselect.simwindow.simcontrol.simulationMap.AddTile(tile);
								}
							}
						}

						#endregion

					}
					/// Set the current building tile to a straight road
					simcontrol.currentBuildTile = new Road(1, 3);
					
					///Set the state to selected
					simcontrol.state = "selected";

					LoadWin.Close();

					/// Set cursor back to an arrow
					this.Cursor = Cursors.Arrow;

					/// New screen
					windowselect.New();			
				}

				/// Throw exception when something is wrong
				catch (Exception ex)
				{
					LoadWin.Close();
					this.Cursor = Cursors.Arrow;

					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}

			}
		}

		/// <summary>
		/// Click About in homescreen
		/// </summary>
 
		public void About_Click()
		{
			// Niewe form met info over 't programma
			about = new AboutWindow();
			about.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            //about.Size = new Size(1000, 1000);

			// Sluit form als dit niet de active form is
			about.Deactivate += About_LostFocus;

			// Open de form
			about.Show();
		}

        /// <summary>
        /// When AboutForm is not the main form anymore
        /// </summary>
        public void About_LostFocus(object sender, EventArgs e)
        {
            /// Close form
            about.Close();
        }

        /// <summary>
        /// Click How-To in homscreen
		/// Opens the manual for the project
        /// </summary>
        public void HowTo_Click()
        {
            /// Adobe Acrobat is geïnstaleerd
            try
            {
                /// Path of the file
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Projectdocument.pdf");

                /// Make new process and open set Adobe Acrobat as the program to open
                Process P = new Process
                {
                    StartInfo = { FileName = "AcroRd32.exe", Arguments = path }
                };

                /// Set process to fullscreen and start the procees
                P.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                P.Start();
            }
            /// Acrobat reader is not installed, the user has to select his own program
            catch
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Projectdocument.pdf");
                string args = "shell32.dll,OpenAs_RunDLL " + path;

                /// Make new process and open the program searcher thing
                Process P = new Process
                {
                    StartInfo = { FileName = "rundll32.exe", Arguments = args }
                };

                /// Set process to fullscreen and start the process
                P.Start();
            }
        }

        /// <summary>
        /// Click Exit in homescreen
		/// Closes the program
        /// </summary>
        public void Exit_Click()
        {
            /// Stop the simulation (if true)
            if (windowselect.simwindow.simcontrol.simulation.simStarted == true)
            {
                windowselect.simwindow.simcontrol.simulation.thread.Abort();
                windowselect.simwindow.simcontrol.simulation.simStarted = false;
            }

            /// Close application
            Application.Exit();


        }
    }
}
