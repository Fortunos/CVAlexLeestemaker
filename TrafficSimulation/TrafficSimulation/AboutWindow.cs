using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TrafficSimulation
{
    public partial class AboutWindow : Form
    {
		InterfaceAbout AboutScherm;
		ElementHost AboutHost;

        public AboutWindow()
        {
            InitializeComponent();

            this.Size = new Size(400, 600);
			this.MinimizeBox = false;
			this.MaximizeBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

			AboutScherm = new InterfaceAbout();

			AboutHost = new ElementHost()
			{
				Height = 600,
				Width = 400,
				Child = AboutScherm
			};

			this.Controls.Add(AboutHost);
        }
    }
}
