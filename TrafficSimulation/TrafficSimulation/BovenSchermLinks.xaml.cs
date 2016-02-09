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

namespace TrafficSimulation
{
    /// <summary>
    /// Interaction logic for BovenScherm.xaml
    /// </summary>
    public partial class BovenSchermLinks : UserControl
    {
        public Boolean InfoVisible = true;
        private Boolean simulation = false;
        InfoBalk infobalk;
        OnderScherm onderscherm;
        WindowSelect windowselect;

        public BovenSchermLinks(WindowSelect ws, InfoBalk info, OnderScherm onder)
        {
            windowselect = ws;
            infobalk = info;
            onderscherm = onder;
            InitializeComponent();

            //At the beginning the simulation part is not used
            play.IsEnabled = false;
            slowDown.IsEnabled = false;
            speedUp.IsEnabled = false;
            stop.IsEnabled = false;
            pauze.IsEnabled = false;
        }

        public Boolean Simulation
        {
            get { return simulation; }
        }
        //Click method to change between simulation and designer
        public void SimulationDesign_Click(object sender, RoutedEventArgs e)
        {
            if (simulation)
            {                
                //Deactivate buttons in bovenschermlinks
                play.IsEnabled = false;
                slowDown.IsEnabled = false;
                speedUp.IsEnabled = false;
                stop.IsEnabled = false;
                pauze.IsEnabled = false;

                //Activate buttons in onderscherm
                onderscherm.selectButton.IsEnabled = true;
                onderscherm.eraserButton.IsEnabled = true;
                onderscherm.roadButton.IsEnabled = true;
                onderscherm.bendButton.IsEnabled = true;
                onderscherm.crossRoadButton.IsEnabled = true;
                onderscherm.forkButton.IsEnabled = true;
                onderscherm.spawnerButton.IsEnabled = true;

                //Stopping movement of cars
                if(windowselect.simwindow.simcontrol.simulation.simStarted)
                    windowselect.simwindow.simcontrol.simulation.StartSim();

                //Removes all cars
                windowselect.simwindow.simcontrol.ClearRoad();
                windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
               
                windowselect.simwindow.simcontrol.Invalidate();
                windowselect.simwindow.InfoBalk.UpdateSimulationReset();
                simulationDesign.Content = "Simulation";
                simulation = false;
                windowselect.simwindow.simcontrol.vehicleBC.AddGrid();
            }
            else
            {
                //windowselect.simwindow.simcontrol.vehicleBC = new BitmapControl(windowselect.simwindow.simcontrol.vehicleBC.bitmap.Size);
                //Activate buttons in bovenschermlinks
                if (windowselect.simwindow.simcontrol.simulation.StartSim())
                {
                    windowselect.simwindow.simcontrol.ClearRoad();
                    play.IsEnabled = false;
                    pauze.IsEnabled = true;
                    stop.IsEnabled = true;
                    slowDown.IsEnabled = true;
                    speedUp.IsEnabled = true;
                    windowselect.simwindow.InfoBalk.UpdateSimulationReset();
                   

                    // Deactivate buttons in onderscherm
                    onderscherm.selectButton.IsEnabled = false;
                    onderscherm.eraserButton.IsEnabled = false;
                    onderscherm.roadButton.IsEnabled = false;
                    onderscherm.bendButton.IsEnabled = false;
                    onderscherm.crossRoadButton.IsEnabled = false;
                    onderscherm.forkButton.IsEnabled = false;
                    onderscherm.spawnerButton.IsEnabled = false;
                    windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);

                    //Make sure you can't draw anymore, so that you can only move
                    windowselect.simwindow.simcontrol.state = "selected";

                    simulationDesign.Content = "Design";
                    simulation = true;
                    windowselect.simwindow.simcontrol.UpdateInfoBalkDesign();
                }
            }
            windowselect.simwindow.simcontrol.trafficlightPB.Invalidate();
        }

        //Click method for starting simulation
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.simulation.StartSimknop();
            play.IsEnabled = false;
            pauze.IsEnabled = true;
            stop.IsEnabled = true;
            slowDown.IsEnabled = true;
            speedUp.IsEnabled = true;
        }

        //Click method for pausing simulation
        private void Pauze_Click(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.simulation.PauseSimknop() ;
            pauze.IsEnabled = false;
            play.IsEnabled = true;
            slowDown.IsEnabled = false;
            speedUp.IsEnabled = false;
        }

        //Click method for stopping simulation
        private void Stop_Clik(object sender, RoutedEventArgs e)
        {
            
            windowselect.simwindow.simcontrol.simulation.StartSim();
            windowselect.simwindow.simcontrol.ClearRoad();
            play.IsEnabled = true;
            pauze.IsEnabled = false;
            stop.IsEnabled = false;
            slowDown.IsEnabled = false;
            speedUp.IsEnabled = false;
            windowselect.simwindow.InfoBalk.UpdateSimulationReset();
            windowselect.simwindow.simcontrol.UpdateInfoBalkDesign();
            
        }

        //Click method for slowing down simulation
        private void SlowDown_Click(object sender, RoutedEventArgs e)
        {
            if (windowselect.simwindow.simcontrol.simulation.extraSpeed != 0)
                windowselect.simwindow.simcontrol.simulation.extraSpeed--;
            else
                windowselect.simwindow.simcontrol.simulation.PauseSeconds += 10;
            windowselect.simwindow.simcontrol.gameSpeed -= 0.1;
        }

        //Click method for accelerating simulation
        private void SpeedUp_Click(object sender, RoutedEventArgs e)
        {
            if (windowselect.simwindow.simcontrol.simulation.PauseSeconds>50)
                windowselect.simwindow.simcontrol.simulation.PauseSeconds -= 10;
            else
                windowselect.simwindow.simcontrol.simulation.extraSpeed++;
            windowselect.simwindow.simcontrol.gameSpeed +=0.1;
        }

    }
}
