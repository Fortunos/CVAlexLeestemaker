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
    /// Interaction logic for InterfaceStart.xaml
    /// </summary>
    public partial class InterfaceStart : UserControl
    {
        StartWindow startwindow;
        int widthStartScreen, heightStartScreen;
        int widthStopGo, buttonMargin, wegMargin, widthWeg, heightWeg;

        public InterfaceStart(StartWindow start, int wSS, int hSS)
        {
            InitializeComponent();
            widthStartScreen = wSS;
            heightStartScreen = hSS;
            startwindow = start;
            resume.Visibility = Visibility.Hidden;

            widthWeg = widthStartScreen / 4;
            heightWeg = heightStartScreen;
            wegMargin = widthStartScreen / 14;
            widthStopGo = widthStartScreen / 9;
            buttonMargin = widthStartScreen / 38;

            weg.Margin = new Thickness(wegMargin, 0, 0, 0);
            weg.Width = widthWeg;
            strepen.Margin = new Thickness((wegMargin + (widthWeg/2) ), 1, 1, 1);
            stopgo.Width = widthStopGo;
            slogan.Margin = new Thickness(widthStopGo, 5, 5, 5);
            resume.Margin = new Thickness((buttonMargin * 2), 5, 5, 5);
            nieuw.Margin = new Thickness((buttonMargin * 3), 5, 5, 5);
            open.Margin = new Thickness((buttonMargin * 4), 5, 5, 5);
            howTo.Margin = new Thickness((buttonMargin * 5), 5, 5, 5);
            about.Margin = new Thickness((buttonMargin * 6), 5, 5, 5);
            exit.Margin = new Thickness((buttonMargin * 7), 5, 5, 5);
            
        }
        public void resume_Click(object sender, RoutedEventArgs e)
        {
            startwindow.Resume_Click();
        }

        public void New_Click(object sender, RoutedEventArgs e)
        {
            startwindow.New_Click();
            resume.Visibility = Visibility.Visible;
            
        }

		public void Open_Click(object sender, RoutedEventArgs e)
		{
			startwindow.Open_Click();
            resume.Visibility = Visibility.Visible;
		}

		public void HowTo_Click(object sender, RoutedEventArgs e)
		{
			startwindow.HowTo_Click();
		}

		public void About_Click(object sender, RoutedEventArgs e)
		{
			startwindow.About_Click();
		}

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            startwindow.Exit_Click();
        }
    }
}