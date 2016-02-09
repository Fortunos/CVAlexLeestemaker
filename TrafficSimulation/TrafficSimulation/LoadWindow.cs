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
	public partial class LoadWindow : Form
	{
		public LoadWindow()
		{
			InitializeComponent();

			progressBar1.Step = 1;
			progressBar1.Value = 0;
			progressBar1.Visible = true;
		}
	}
}
