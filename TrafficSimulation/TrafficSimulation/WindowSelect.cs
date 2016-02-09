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
    public partial class WindowSelect : Form
    {
        StartWindow startwindow;
        public SimWindow simwindow;
        public Size screensize;

        public WindowSelect()
        {
            int widthStartScreen, heightStartScreen;

            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //Schermgroote bepalen
            using (Graphics graphics = this.CreateGraphics()) 
            {
                widthStartScreen = Screen.PrimaryScreen.Bounds.Width;
                heightStartScreen = Screen.PrimaryScreen.Bounds.Height ;
            }

            screensize = new Size(widthStartScreen, heightStartScreen);
            simwindow = new SimWindow(screensize, this);
            startwindow = new StartWindow(screensize, this);
            Start();
        }

		// Open the homescreen.
        public void Start()
        {
            this.Controls.Remove(simwindow);
           
            // Open homescreen
            startwindow.BackColor = Color.Black;

			// Add the control
            this.Controls.Add(startwindow);
        }


		// Create a new field for the traffic simulation.
        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(startwindow);

            /// Open simwindow
	        this.Controls.Add(simwindow);
        }


        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case (Keys.Control | Keys.S):
					this.simwindow.BovenSchermRechts.Save_Click(null, null);
					return true;

				case (Keys.Control | Keys.H):
					this.simwindow.BovenSchermRechts.Home_Click(null, null);
					return true;

				case (Keys.Control | Keys.I):
					this.simwindow.BovenSchermRechts.Info_Click(null, null);
					return true;

                case (Keys.Alt | Keys.F4):
                    WindowSelect_FormClosing(null, null);
                    return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

        private void WindowSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(simwindow.simcontrol.simulation.simStarted)
                simwindow.simcontrol.simulation.StartSim();
            this.Close();

        }
    }
}
