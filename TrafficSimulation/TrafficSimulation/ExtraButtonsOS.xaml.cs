using System;
using System.Collections.Generic;
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
    /// Interaction logic for ExtraButtonsOnderScherm.xaml
    /// </summary>
    public partial class ExtraButtonsOS : UserControl
    {
        InfoBalk Infobalk;
        WindowSelect windowselect;
        public Boolean mouseEntered;
  
        public ExtraButtonsOS(WindowSelect ws, InfoBalk info)
        {
            windowselect = ws;
            Infobalk = info;
            InitializeComponent();
            mouseEntered = false;
        }

        private void LostFocusExtra(object sender, RoutedEventArgs e)
        {
            HideButtons();
        }
        
        //Click method straight road 1 to 3 (vertical)
        private void Roadver_Checked(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 3);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.roadButton.IsChecked = true;
        }

        //Click method straight road 2 to 4 (horizontal)
        private void Roadhor_Checked(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(2, 4);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.roadButton.IsChecked = true;
        }

        //Click method curved road 1 to 2
        private void Curve12_Checked(object sender, RoutedEventArgs e)
        {
            HideButtons(); 
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 2);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.bendButton.IsChecked = true;
        }

        //Click method curved road 2 to 3
        private void Curve23_Checked(object sender, RoutedEventArgs e)
        {
            HideButtons(); 
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(2, 3);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.bendButton.IsChecked = true;
        }

        //Click method curved road 3 to 4
        private void Curve34_Checked(object sender, RoutedEventArgs e)
        {
            HideButtons(); 
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(3, 4);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.bendButton.IsChecked = true;
        }

        //Click method curved road 1 to 4
        private void Curve14_Checked(object sender, RoutedEventArgs e)
        {
            HideButtons(); 
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 4);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.bendButton.IsChecked = true;
        }

        //Click method fork 1 to 2
        private void Fork12_Click(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Fork(windowselect.simwindow.simcontrol, 4);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.forkButton.IsChecked = true;
        }

        //Click method fork 2 to 3
        private void Fork23_Click(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Fork(windowselect.simwindow.simcontrol,3);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.forkButton.IsChecked = true;
        }

        //Click method fork 3 to 4
        private void Fork34_Click(object sender, RoutedEventArgs e)
        {
            HideButtons(); 
            windowselect.simwindow.simcontrol.currentBuildTile = new Fork(windowselect.simwindow.simcontrol, 2);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.forkButton.IsChecked = true;
        }

        //Click method fork 1 to 4
        private void Fork14_Click(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Fork(windowselect.simwindow.simcontrol, 1);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.forkButton.IsChecked = true;
        }

        //Click method spawner down
        private void Spawnerdown_Click(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Spawner(windowselect.simwindow.simcontrol,1);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.spawnerButton.IsChecked = true;
        }

        //Click method spawner left
        private void Spawnerleft_Click(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Spawner(windowselect.simwindow.simcontrol, 2);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.spawnerButton.IsChecked = true;
        }

        //Click method spawner up
        private void Spawnerup_Click(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Spawner(windowselect.simwindow.simcontrol, 3);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.spawnerButton.IsChecked = true;
        }

        //Click method spawner right
        private void Spawnerright_Click(object sender, RoutedEventArgs e)
        {
            HideButtons();
            windowselect.simwindow.simcontrol.currentBuildTile = new Spawner(windowselect.simwindow.simcontrol, 4);
            windowselect.simwindow.simcontrol.state = "building";
            windowselect.simwindow.OnderScherm.spawnerButton.IsChecked = true;
        }

        private void HideButtons()
        {
            this.Visibility = Visibility.Hidden;
            this.roadhor.Visibility = Visibility.Hidden;
            this.roadver.Visibility = Visibility.Hidden;
            this.bend12.Visibility = Visibility.Hidden;
            this.bend23.Visibility = Visibility.Hidden;
            this.bend34.Visibility = Visibility.Hidden;
            this.bend14.Visibility = Visibility.Hidden;
            this.fork12.Visibility = Visibility.Hidden;
            this.fork23.Visibility = Visibility.Hidden;
            this.fork34.Visibility = Visibility.Hidden;
            this.fork14.Visibility = Visibility.Hidden;
            this.spawnerdown.Visibility = Visibility.Hidden;
            this.spawnerleft.Visibility = Visibility.Hidden;
            this.spawnerup.Visibility = Visibility.Hidden;
            this.spawnerright.Visibility = Visibility.Hidden;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
            mouseEntered = false;

            
            
        }

        private void Mouse_Enter(object sender, MouseEventArgs e)
        {
            mouseEntered = true;
        }
        public bool ContainsMouse()
        {
            if (this.IsMouseOver)
                return true;
            return false;
        }
               
    }
}
