using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficSimulation
{
    public partial class EfficientieWindow : Form
    {
        DateTime EndOfTime;
        Boolean timerStarted;
        Timer t = new Timer() { Interval = 1, Enabled = false };
        TimeSpan endTime;
        SimWindow simwindow;

        public EfficientieWindow(SimWindow sim)
        {
            this.simwindow = sim;
            InitializeComponent();
            button1.Enabled = false;
            button3.Enabled = false;
            button2.Tag = "paused";
            timerStarted = false;
            label3.Text = "";
        }

        //stopbutton
        private void button1_Click(object sender, EventArgs e)
        {
            //resetting the window
            button2.BackgroundImage = new Bitmap((Bitmap)TrafficSimulation.Properties.Resources.ResourceManager.GetObject("play_button"));
            button2.Tag = "paused";
            button1.Enabled = false;
            timerStarted = false;
            t.Stop();
            EnableHosts(true);
            label3.Text = "";
            label2.Text = "Enter a time to calculate the efficiency";
            simwindow.simcontrol.simulation.StartSim();
            simwindow.simcontrol.ClearRoad();
            progressBar1.Value = 0;

        }
        //playbutton
        private void button2_Click(object sender, EventArgs e)
        {   //start counting 
            if ((string)button2.Tag == "paused")
            {
                button2.BackgroundImage = new Bitmap((Bitmap)TrafficSimulation.Properties.Resources.ResourceManager.GetObject("Pause_Button"));
                simwindow.simcontrol.ResetSimulationCounters();
                button1.Enabled = true;
                button2.Tag = "playing";
                StartTimer();
                EnableHosts(false);
                label2.Text = "Remaining time:";
                timerStarted = true;
                simwindow.simcontrol.simulation.StartSimknop();
                
            }
            else
            {   //pause the counting and the simulation
                button2.BackgroundImage = new Bitmap((Bitmap)TrafficSimulation.Properties.Resources.ResourceManager.GetObject("play_button"));
                button2.Tag = "paused";
                t.Stop();
                simwindow.simcontrol.simulation.PauseSimknop();
            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            MessageBox.Show("please use a valid time");
        }

        //save results button
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // New savedialog
				System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

				// Custom filename
				int number = 1;
				string fileName = "TrafficEfficiency" + number.ToString();
				
				// Extension name (.trs => TRafficSimulation)
				saveDialog.DefaultExt = ".trs";
				saveDialog.FilterIndex = 1;
				saveDialog.Filter = "Traffic Simulation Files (*.trs) | *.trs";
				saveDialog.RestoreDirectory = true;
				saveDialog.FileName = fileName;

				// Set the ability to overwrite another file to true
				saveDialog.OverwritePrompt = true;

				// Is the button "Save" pressed?
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					/// New file
					System.IO.StreamWriter file = new System.IO.StreamWriter(@saveDialog.FileName);

					/// File can be bigger than 1024
					file.AutoFlush = true;
                    
                    List<int> numbers = new List<int>();
                    foreach (string[] n in simwindow.simcontrol.simulation.efficiencyNumbers)
                    {
                        file.WriteLine(n[0] + "  :  " + n[1]);
                    }
				}
			}
			/// Throw an exception
			catch (Exception exp)
			{
				MessageBox.Show("Error: Could not write file to disk. Original error:" + exp);
			}
            label2.Text = "Enter a time to calculate the efficiency";
            label3.Text = "";
		}	
        
        private void StartTimer()
        {
            if (timerStarted)
            {
                endTime =new TimeSpan(endTime.Ticks-( new TimeSpan(endTime.Ticks - TimeSpan.Parse(label3.Text).Ticks)).Ticks);
                EndOfTime = DateTime.Now.Add(endTime);
            }
            else
            {
                string time = maskedTextBox1.Text;
                try
                {
                    endTime = TimeSpan.Parse(time);
                    EndOfTime = DateTime.Now.Add(endTime);
                }
                catch
                {
                    try
                    {
                        string[] timePart = time.Split(':');
                        time = "00:" + timePart[1] + ":" + timePart[2];
                        endTime = TimeSpan.Parse(time);
                        EndOfTime = DateTime.Now.Add(endTime);
                    }
                    catch
                    {
                        time = "00:05:00";
                        endTime = TimeSpan.Parse(time);
                        EndOfTime = DateTime.Now.AddMinutes(5);

                    }
                }
                progressBar1.Maximum = endTime.Seconds + (60 * endTime.Minutes) + (3600 * endTime.Hours);
            }
            t.Start();
            t_Tick(null, null);
        }

        public void t_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = EndOfTime.Subtract(DateTime.Now);
            label3.Text = ts.ToString(@"hh\:mm\:ss");
            int newValue = progressBar1.Maximum -(ts.Seconds + (60 * ts.Minutes) + (3600 * ts.Hours));
            if (newValue < progressBar1.Maximum)
                progressBar1.Value = newValue;
            else if(simwindow.simcontrol.totalCars!=0)
            {
                
                ShowEfficiency();
                
            }

        }

        private void ShowEfficiency()
        {
            t.Stop();
            progressBar1.Value = progressBar1.Maximum;
            button2.BackgroundImage = new Bitmap((Bitmap)TrafficSimulation.Properties.Resources.ResourceManager.GetObject("play_button"));
            button2.Tag = "paused";
            timerStarted = false;
            int average = CalculateEfficiency();
            label3.Text = "";
            label2.Text = "The efficiency of your map is: " + average + "%";
            button3.Enabled = true;
            simwindow.simcontrol.simulation.StartSim();
            simwindow.simcontrol.ClearRoad();
        }
        private int CalculateEfficiency()
        {
            List<int> numbers = new List<int>();
            foreach (string[] n in simwindow.simcontrol.simulation.efficiencyNumbers)
                numbers.Add(int.Parse(n[1]));
            return (int)numbers.Average();
        }

        private void EfficientieWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(button1.Enabled== true)
                simwindow.simcontrol.simulation.StartSim();
            
            
            EnableHosts(true);
            simwindow.BovenSchermLinks.SimulationDesign_Click(null, null);
            simwindow.BovenSchermLinks.SimulationDesign_Click(null, null);
        }
        private void EnableHosts(Boolean enabled)
        {
            simwindow.bovenHostLinks.Enabled = enabled;
            simwindow.bovenHostRechts.Enabled = enabled;
            simwindow.onderHost.Enabled = enabled;
            simwindow.infoHost.Enabled = enabled;
        }

        private void StartEfficiency(object sender, EventArgs e)
        {
            if(simwindow.BovenSchermLinks.Simulation == true)
            simwindow.BovenSchermLinks.SimulationDesign_Click(null, null);
        }
    }
}
