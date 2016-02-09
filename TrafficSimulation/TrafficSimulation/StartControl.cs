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
    public partial class StartControl : UserControl
    {
        ElementHost StartHost;
        SimWindow simwindow;
        

        public StartControl(Size size, SimWindow sim)
        {
            InterfaceStart StartScherm = new InterfaceStart(this); 
            this.Size = size;
            simwindow = sim;

            StartHost = new ElementHost()
            {
                Location = new Point(((size.Width-300)/2), ((size.Height -300)/2)),
                Height = 300,
                Width = 300,
                BackColor = Color.Green,
                Child = StartScherm
            };
            this.Controls.Add(StartHost);

        }

        public void New_Click()
        {
            // Open simcontrol
            simwindow.New();
        }

        public void Option_Click()
        {

        }

        public void Exit_Click()
        {
            // Sluit applicatie
            Application.Exit();
        }
    }
}
