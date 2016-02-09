using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TrafficSimulation
{
    public partial class SimWindow : UserControl
    {
        SimControl sim;
        public WindowSelect windowselect;
        public ElementHost bovenHostLinks, bovenHostRechts, onderHost, infoHost, extraButtonsHost;
        public BovenSchermLinks BovenSchermLinks;
        public BovenSchermRechts BovenSchermRechts;
        public InfoBalk InfoBalk;
        public ExtraButtonsOS ExtraButtonsOS;
        public OnderScherm OnderScherm;
        public int hoogteBovenBalk, hoogteOnderBalk, hoogteInfoBalk, hoogteScherm, yLocatieOnderBalk, xLocatieOnderBalk, yLocatieBovenSchermRechts;
        public int breedteInfoBalk, breedteScherm, breedteBovenSchermLinks, breedteBovenSchermRechts, breedteOnderBalk;

        public SimWindow(Size size, WindowSelect windowselect)
        {
            this.Size = size;
            this.windowselect = windowselect;
            sim = new SimControl(this.ClientSize, this);
            this.BackColor = Color.Green;

            //Variable om de elementhosten afhankelijk te maken van het scherm en andere elementhosten
            breedteScherm = Screen.PrimaryScreen.Bounds.Width;
            hoogteScherm = Screen.PrimaryScreen.Bounds.Height;
            hoogteBovenBalk = 80;
            hoogteOnderBalk = 100;
            hoogteInfoBalk = (hoogteScherm - (hoogteBovenBalk + hoogteOnderBalk));
            yLocatieOnderBalk = (hoogteScherm - hoogteOnderBalk);
            xLocatieOnderBalk = (breedteScherm / 7) * 2;
            breedteInfoBalk = breedteScherm / 6;
            breedteOnderBalk = ((breedteScherm / 3));

            using (Graphics graphics = this.CreateGraphics())
            {
                breedteBovenSchermLinks = (260 * (int)graphics.DpiX) / 96;
                breedteBovenSchermRechts = ((55 * 4) * (int)graphics.DpiX) / 96;
                breedteInfoBalk = ((300) * (int)graphics.DpiX) / 96;
                xLocatieOnderBalk = (((breedteScherm / 8) * 2) * (int)graphics.DpiX) / 96;
            }

            InfoBalk = new InfoBalk(windowselect);
            ExtraButtonsOS = new ExtraButtonsOS(windowselect, InfoBalk);
            OnderScherm = new OnderScherm(windowselect, InfoBalk, ExtraButtonsOS, extraButtonsHost, breedteOnderBalk, yLocatieOnderBalk, xLocatieOnderBalk, hoogteOnderBalk);
            BovenSchermLinks = new BovenSchermLinks(windowselect, InfoBalk, OnderScherm);
            BovenSchermRechts = new BovenSchermRechts(windowselect, InfoBalk, OnderScherm, breedteScherm, breedteInfoBalk, hoogteBovenBalk);

            extraButtonsHost = new ElementHost()
            {
                Height = 200,
                Width = 100,
                Location = new Point(this.Size),
                Child = ExtraButtonsOS,
            };
            this.Controls.Add(extraButtonsHost);

            bovenHostLinks = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = hoogteBovenBalk,
                Width = breedteBovenSchermLinks,
                Location = new Point(10, 10),
                Child = BovenSchermLinks,
            };
            this.Controls.Add(bovenHostLinks);

            bovenHostRechts = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = hoogteBovenBalk,
                Width = breedteBovenSchermRechts,
                Location = new Point((breedteScherm - breedteBovenSchermRechts), 0),
                Child = BovenSchermRechts,
            };
            this.Controls.Add(bovenHostRechts);

            onderHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(xLocatieOnderBalk, yLocatieOnderBalk),
                Height = hoogteOnderBalk,
                Width = breedteOnderBalk,
                Child = OnderScherm,
            };
            this.Controls.Add(onderHost);

            infoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(this.Size),
                Height = hoogteInfoBalk,
                Width = breedteInfoBalk,
                Child = InfoBalk,
            };
            this.Controls.Add(infoHost);

            this.Controls.Add(sim);

        }
        public SimControl simcontrol { get { return sim; } }
    }
}
