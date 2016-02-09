using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for InfoBalk.xaml
    /// </summary>
    public partial class InfoBalk : UserControl
    {
        WindowSelect windowselect;
        public EfficientieWindow EfWindow;

        public InfoBalk(WindowSelect ws)
        {
            windowselect = ws;
            InitializeComponent();
            HideCombobox();
        }

        private void lane1_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane1.Tag, ((ComboBoxItem)lane1.SelectedItem).ToString());
        }

        private void lane2_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane2.Tag, ((ComboBoxItem)lane2.SelectedItem).ToString());
        }

        private void lane3_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane3.Tag, ((ComboBoxItem)lane3.SelectedItem).ToString());
        }

        private void lane4_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane4.Tag, ((ComboBoxItem)lane4.SelectedItem).ToString());
        }

        private void lane5_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane5.Tag, ((ComboBoxItem)lane5.SelectedItem).ToString());
        }

        private void lane6_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane6.Tag, ((ComboBoxItem)lane6.SelectedItem).ToString());
        }

        private void lane7_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane7.Tag, ((ComboBoxItem)lane7.SelectedItem).ToString());
        }

        private void lane8_Close(object sender, EventArgs e)
        {
            ChangeCasts((string)lane8.Tag, ((ComboBoxItem)lane8.SelectedItem).ToString());
        }

        private void speed_Close(object sender, EventArgs e)
        {
            int speed1 = speed.SelectedIndex;
            windowselect.simwindow.simcontrol.selectedTile.MaxSpeed = speed1 + 2;
        }

        private void SpawnerHigh_Checked(object sender, EventArgs e)
        {
            Spawner spawner = (Spawner)windowselect.simwindow.simcontrol.selectedTile;
            spawner.CarsSpawnChance = 1;
        }

        private void SpawnerLow_Checked(object sender, EventArgs e)
        {
            Spawner spawner = (Spawner)windowselect.simwindow.simcontrol.selectedTile;
            spawner.CarsSpawnChance = 5;
        }

        private void SpawnerNormal_Checked(object sender, EventArgs e)
        {
            Spawner spawner = (Spawner)windowselect.simwindow.simcontrol.selectedTile;
            spawner.CarsSpawnChance = 3;
        }

        private void ChangeCasts(string kant, string ob)
        {
            string[] kantEnPlaats = kant.Split('_');
            string[] array = ob.Split(' ');
            ChangeTileLanes(int.Parse(array[1]), int.Parse(kantEnPlaats[0]), int.Parse(kantEnPlaats[1]));
        }

        private void ChangeTileLanes(int value, int side, int inOrOut)
        {
            if (lane1.SelectedItem != null)
            {

                if (windowselect.simwindow != null)
                {
                    if (windowselect.simwindow.simcontrol.selectedTile != null && windowselect.simwindow.simcontrol.selectedTile.name != "Fork" && windowselect.simwindow.simcontrol.selectedTile.name != "Crossroad")
                        if (inOrOut == 0)
                            windowselect.simwindow.simcontrol.selectedTile.UpdateLanes(windowselect.simwindow.simcontrol, side, value, windowselect.simwindow.simcontrol.selectedTile.GetLanesOut(side));
                        else
                            windowselect.simwindow.simcontrol.selectedTile.UpdateLanes(windowselect.simwindow.simcontrol, side, windowselect.simwindow.simcontrol.selectedTile.GetLanesIn(side), value);
                    windowselect.simwindow.simcontrol.selectedTile.UpdateOtherTiles(windowselect.simwindow.simcontrol, 0);
                    windowselect.simwindow.simcontrol.backgroundPB.Invalidate();
                    windowselect.simwindow.simcontrol.UpdateInfoBalkDesign();
                    windowselect.simwindow.simcontrol.DrawSelectLine(windowselect.simwindow.simcontrol.selectedTile.position);
                }
            }
        }

        //copied from internet
        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {

            }

            return bs;
        }

        public void UpdateDesign(int[,] tileLanes, int maxSpeed,int totalTiles, int trafficlights,int strategie,double gameSpeed)
        {
            HideCombobox();
            speed.Visibility = Visibility.Visible;
            RotatedRight.Visibility = Visibility.Visible;
            RotateLeft.Visibility = Visibility.Visible;
            labelSpeed.Visibility = Visibility.Visible;
            if(windowselect.simwindow.BovenSchermLinks.Simulation)
            {
                RotatedRight.IsEnabled = false;
                RotateLeft.IsEnabled = false;
                if (windowselect.simwindow.simcontrol.simulation.simStarted )
                {
                    speed.IsEnabled = false;
                    listboxStrategie.IsEnabled = false;
                    
                }
            }
            Boolean CrosOrFork = false;
            if (windowselect.simwindow.simcontrol.selectedTile.name == "Crossroad" || windowselect.simwindow.simcontrol.selectedTile.name == "Fork")
                CrosOrFork = true;
            ShowComboBox(windowselect.simwindow.simcontrol.selectedTile.Directions, CrosOrFork);
            ImageInfoBalk.Source = loadBitmap(windowselect.simwindow.simcontrol.selectedTile.DrawImage());
            speed.SelectedIndex = maxSpeed - 2;
            lane3.SelectedIndex = tileLanes[0, 1] - 1;
            lane4.SelectedIndex = tileLanes[0, 0] - 1;
            lane5.SelectedIndex = tileLanes[1, 1] - 1;
            lane6.SelectedIndex = tileLanes[1, 0] - 1;
            lane7.SelectedIndex = tileLanes[2, 1] - 1;
            lane8.SelectedIndex = tileLanes[2, 0] - 1;
            lane1.SelectedIndex = tileLanes[3, 1] - 1;
            lane2.SelectedIndex = tileLanes[3, 0] - 1;
            labelCrossroadsNumber.Content = trafficlights;
            labelTilesNumber.Content = totalTiles;
            listboxStrategie.SelectedIndex = strategie;
            labelGameSpeedNumber.Content = Math.Round(gameSpeed, 1); 
        }

        public void HideCombobox()
        {
            ImageInfoBalk.Source = null;
            // Let all combo boxes disappear
            lane1.Visibility = Visibility.Hidden;
            lane2.Visibility = Visibility.Hidden;
            lane3.Visibility = Visibility.Hidden;
            lane4.Visibility = Visibility.Hidden;
            lane5.Visibility = Visibility.Hidden;
            lane6.Visibility = Visibility.Hidden;
            lane7.Visibility = Visibility.Hidden;
            lane8.Visibility = Visibility.Hidden;

            //Make anything not editable
            lane1.IsEnabled = false;
            lane2.IsEnabled = false;
            lane3.IsEnabled = false;
            lane4.IsEnabled = false;
            lane5.IsEnabled = false;
            lane6.IsEnabled = false;
            lane7.IsEnabled = false;
            lane8.IsEnabled = false;

            speed.Visibility = Visibility.Hidden;
            RotatedRight.Visibility = Visibility.Hidden;
            RotateLeft.Visibility = Visibility.Hidden;
            labelSpeed.Visibility = Visibility.Hidden;

            RotatedRight.IsEnabled = true;
            RotateLeft.IsEnabled = true;
            speed.IsEnabled = true;
            listboxStrategie.IsEnabled = true;
            spawnerCarHigh.Visibility = Visibility.Hidden;
            spawnerCarsNormal.Visibility = Visibility.Hidden;
            spawnerCarsLow.Visibility = Visibility.Hidden;
            labelStrategie.Visibility = Visibility.Hidden;
            listboxStrategie.Visibility = Visibility.Hidden;
            
        }

        public void ShowComboBox(List<int> Directions, Boolean CrossOrFork)
        {
            Boolean simulationStarted = windowselect.simwindow.simcontrol.simulation.simStarted;
            if(CrossOrFork)
            {
                labelStrategie.Visibility = Visibility.Visible;
                listboxStrategie.Visibility = Visibility.Visible;
            }
            if (Directions.Contains(1))
            {
                lane3.Visibility = Visibility.Visible;
                lane4.Visibility = Visibility.Visible;
                if (!CrossOrFork && !simulationStarted)
                {
                    lane3.IsEnabled = true;
                    lane4.IsEnabled = true;
                }
            }
            if (Directions.Contains(2))
            {
                lane5.Visibility = Visibility.Visible;
                lane6.Visibility = Visibility.Visible;
                if (!CrossOrFork && !simulationStarted)
                {
                    lane5.IsEnabled = true;
                    lane6.IsEnabled = true;
                }
            }
            if (Directions.Contains(3))
            {
                lane7.Visibility = Visibility.Visible;
                lane8.Visibility = Visibility.Visible;
                if (!CrossOrFork && !simulationStarted)
                {
                    lane7.IsEnabled = true;
                    lane8.IsEnabled = true;
                }
            }
            if (Directions.Contains(4))
            {
                lane1.Visibility = Visibility.Visible;
                lane2.Visibility = Visibility.Visible;
                if (!CrossOrFork && !simulationStarted)
                {
                    lane1.IsEnabled = true;
                    lane2.IsEnabled = true;
                }
            }
            if(windowselect.simwindow.simcontrol.selectedTile.name=="Spawner")
            {
                spawnerCarHigh.Visibility = Visibility.Visible;
                spawnerCarsNormal.Visibility = Visibility.Visible;
                spawnerCarsLow.Visibility = Visibility.Visible;
                Spawner spawner = (Spawner)windowselect.simwindow.simcontrol.selectedTile;
                switch(spawner.CarsSpawnChance)
                {
                    case 5: spawnerCarsLow.IsChecked = true;
                        break;
                    case 3: spawnerCarsNormal.IsChecked = true;
                        break;
                    case 1: spawnerCarHigh.IsChecked = true;
                        break;

                }
            }
        }

        public void UpdateSimulation(int totalCars, int WaitingCars,int TileCars,double gameSpeed)
        {
            int DrivingCars = totalCars - WaitingCars;
            labelTotalCarsNumber.Content = totalCars;
            labelWaitingCarsNumber.Content = WaitingCars;
            labelDrivingCarsNumber.Content = DrivingCars;
            labelCarsOnTileNumber.Content = TileCars;
            labelGameSpeedNumber.Content = Math.Round(gameSpeed,1);
            int efficiency = (int)((double)((double)DrivingCars / (totalCars))*100);
            if (efficiency < 101 && efficiency>-1)
                labelEfficientieNumber.Content = efficiency;
            else
                labelEfficientieNumber.Content = 100;
            if(windowselect.simwindow.simcontrol.selectedTile != null && lane1.Visibility == Visibility.Hidden)
            {
                windowselect.simwindow.simcontrol.UpdateInfoBalkDesign();
            }
        }

        public void UpdateSimulationReset()
        {
            labelTotalCarsNumber.Content = 0;
            labelWaitingCarsNumber.Content = 0;
            labelDrivingCarsNumber.Content = 0;
            labelCarsOnTileNumber.Content = 0;
            labelGameSpeedNumber.Content = 1;
            labelEfficientieNumber.Content = 0;
            windowselect.simwindow.simcontrol.ResetSimulationCounters();
        }

        private void RotateLeftClick(object sender, RoutedEventArgs e)
        {
            RotateTile("Right");
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            RotateTile("Left");
        }

        private void RotateTile(string LeftOrRight)
        {
            int difference;
            if (LeftOrRight == "Right")
                difference = 0;
            else
                difference = +2;

            Tile originalTile = windowselect.simwindow.simcontrol.selectedTile;
            SimControl simcontrol = windowselect.simwindow.simcontrol;
            Tile rotatedTile;
            switch (originalTile.name)
            {
                case "Fork": rotatedTile = new Fork(simcontrol, (originalTile.NotDirection+difference) % 4 + 1);
                    break;
                case "Crossroad": rotatedTile = new Crossroad(simcontrol);
                    break;
                case "Road": rotatedTile = new Road((originalTile.StartDirection+difference )% 4 + 1, (originalTile.EndDirection +difference)% 4 + 1);
                    break;
                case "Spawner": rotatedTile = new Spawner(windowselect.simwindow.simcontrol, (originalTile.Direction + difference) % 4 + 1);
                    break;
                default: rotatedTile = new Crossroad(simcontrol);
                    break;
            }
            simcontrol.DrawTile(originalTile.position, rotatedTile);
            windowselect.simwindow.simcontrol.DrawSelectLine(originalTile.position);

        }

        private void listboxStrategie_Closing(object sender, EventArgs e)
        {
            Tile selectedTile = windowselect.simwindow.simcontrol.selectedTile;
            int strategie = listboxStrategie.SelectedIndex;
            if (selectedTile.name == "Crossroad")
            {
                Crossroad crosTile = (Crossroad)selectedTile;
                crosTile.control.strat = strategie;
            }
            else
            {
                Fork crosTile = (Fork)selectedTile;
                crosTile.control.strat = strategie;
            }
        }

        private void Efficientie_Click(object sender, RoutedEventArgs e)
        {
            EfWindow = new EfficientieWindow(windowselect.simwindow);

            EfWindow.Show();
            EfWindow.TopMost = true;
        }
    }
}
