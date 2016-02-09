using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Forms.Integration;



namespace TrafficSimulation
{
    /// <summary>
    /// Interaction logic for OnderScherm.xaml
    /// </summary>
    public partial class OnderScherm : UserControl
    {
        WindowSelect windowselect;
        InfoBalk infoBalk;
        ExtraButtonsOS extraButtonsOS;
        ElementHost extraButtonsHost;
        int breedteOnderBalk, yLocatieOnderBalk, xLocatieOnderBalk, hoogteOnderBalk;
        

        public OnderScherm(WindowSelect ws, InfoBalk info, ExtraButtonsOS extra, ElementHost extraHost, int bob, int ylob, int xlob, int hob)
        {
            windowselect = ws;
            extraButtonsHost = extraHost;
            infoBalk = info;
            extraButtonsOS = extra;
            breedteOnderBalk = bob;
            yLocatieOnderBalk = ylob;
            xLocatieOnderBalk = xlob;
            hoogteOnderBalk = hob;
            InitializeComponent();
        }

        private void SelectButton_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.state = "selected";           
        }

        private void EraserButton_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.state = "eraser";
		}


        private void RoadButton_Checked(object sender, RoutedEventArgs e)
        {
            AlgemeenClick();
            int hoogteExtraButtonOSRoad = 150 ;
            int xLocationRoadMenu = (xLocatieOnderBalk + (breedteOnderBalk / 6));
            int yLocationRoadMenu = yLocatieOnderBalk - hoogteExtraButtonOSRoad;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSRoad;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationRoadMenu, yLocationRoadMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.roadhor.Visibility = Visibility.Visible;
            extraButtonsOS.roadver.Visibility = Visibility.Visible;
            
            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }
        
        private void BendButton_Checked(object sender, RoutedEventArgs e)
        {
            AlgemeenClick();
            int hoogteExtraButtonOSBend = 300;
            int xLocationBendMenu = (xLocatieOnderBalk + ((breedteOnderBalk / 6) * 2));
            int yLocationBendMenu = yLocatieOnderBalk - hoogteExtraButtonOSBend;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSBend;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationBendMenu, yLocationBendMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.bend12.Visibility = Visibility.Visible;
            extraButtonsOS.bend23.Visibility = Visibility.Visible;
            extraButtonsOS.bend34.Visibility = Visibility.Visible;
            extraButtonsOS.bend14.Visibility = Visibility.Visible;

            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
       }

       private void CrossRoadButton_Checked(object sender, RoutedEventArgs e)
        {
            AlgemeenClick();
            windowselect.simwindow.simcontrol.currentBuildTile = new Crossroad(windowselect.simwindow.simcontrol);
			windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);

			windowselect.simwindow.simcontrol.state = "building";
        }

        private void ForkButton_Checked(object sender, RoutedEventArgs e)
        {
            AlgemeenClick();
            int hoogteExtraButtonOSFork = 300;
            int xLocationForkMenu = (xLocatieOnderBalk + ((breedteOnderBalk / 6) * 4));
            int yLocationForkMenu = yLocatieOnderBalk - hoogteExtraButtonOSFork;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSFork;

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.fork12.Visibility = Visibility.Visible;
            extraButtonsOS.fork14.Visibility = Visibility.Visible;
            extraButtonsOS.fork23.Visibility = Visibility.Visible;
            extraButtonsOS.fork34.Visibility = Visibility.Visible;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationForkMenu, yLocationForkMenu);
        }
        
        private void SpawnerButton_Checked(object sender, RoutedEventArgs e)
        {   
            AlgemeenClick();
            int hoogteExtraButtonOSSpawner = 300;
            int xLocationSpawnerMenu = (xLocatieOnderBalk + ((breedteOnderBalk / 6) * 5));
            int yLocationSpawnerMenu = yLocatieOnderBalk - hoogteExtraButtonOSSpawner;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSSpawner;
            
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationSpawnerMenu, yLocationSpawnerMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerdown.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerleft.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerup.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerright.Visibility = Visibility.Visible;
           
            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }
        private void Lost_Focus(object sender, RoutedEventArgs e) 
        {
            if(!windowselect.simwindow.ExtraButtonsOS.ContainsMouse())
                AlgemeenClick();
        }

     private void AlgemeenClick()
        {
            
            
            extraButtonsOS.Visibility = Visibility.Hidden;
            extraButtonsOS.roadhor.Visibility = Visibility.Hidden;
            extraButtonsOS.roadver.Visibility = Visibility.Hidden;
            extraButtonsOS.bend12.Visibility = Visibility.Hidden;
            extraButtonsOS.bend23.Visibility = Visibility.Hidden;
            extraButtonsOS.bend34.Visibility = Visibility.Hidden;
            extraButtonsOS.bend14.Visibility = Visibility.Hidden;
            extraButtonsOS.fork12.Visibility = Visibility.Hidden;
            extraButtonsOS.fork23.Visibility = Visibility.Hidden;
            extraButtonsOS.fork34.Visibility = Visibility.Hidden;
            extraButtonsOS.fork14.Visibility = Visibility.Hidden;
            extraButtonsOS.spawnerdown.Visibility = Visibility.Hidden;
            extraButtonsOS.spawnerleft.Visibility = Visibility.Hidden;
            extraButtonsOS.spawnerup.Visibility = Visibility.Hidden;
            extraButtonsOS.spawnerright.Visibility = Visibility.Hidden;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
        }

    }
}
