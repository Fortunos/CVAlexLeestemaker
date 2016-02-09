using System.Drawing;
using System.Windows.Forms;

namespace TrafficSimulation
{
    partial class SimControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SimControl";
            this.ResumeLayout(false);
        #endregion

            #region Code for creating all of the objects
            /*
             * PictureBox in which the background is saved, all the tiles in the simulation will be placed on the bitmap
             * in this PictureBox.
             */
            
            Point bitmapLocation = new Point(0,0);
            this.backgroundPB = new PictureBox();
            this.backgroundPB.Image = backgroundBC.bitmap;
            this.backgroundPB.BackColor = Color.Transparent;
            this.backgroundPB.Location = bitmapLocation;
            this.backgroundPB.Size = new Size(this.Width, this.Height);
            /*
             * PictureBox in which the vehicles are saved, all the vehicles in the simulation will be placed on the bitmap
             * in this PictureBox. When the vehicles start to move the bitmaps will also move across the bitmap.
             */
            this.vehiclePB = new PictureBox();
            this.vehiclePB.Image = vehicleBC.bitmap;
            this.vehiclePB.BackColor = Color.Transparent;
            this.vehiclePB.Location = bitmapLocation;
            this.vehiclePB.Size = new Size(this.Width, this.Height);
            /*
             * PictureBox in which the trafficlights are saved, all the trafficlights in the simulation will be placed on the 
             * bitmap in this PictureBox. When the trafficlights start to change color, the bitmap will update the lightcolors. 
             */
            this.trafficlightPB = new PictureBox();
            this.trafficlightPB.Image = trafficlightBC.bitmap;
            this.trafficlightPB.BackColor = Color.Transparent;
            this.trafficlightPB.Location = bitmapLocation;
            this.trafficlightPB.Size = new Size(this.Width, this.Height);

            this.Controls.Add(backgroundPB);
            this.backgroundPB.Controls.Add(vehiclePB);
            this.vehiclePB.Controls.Add(trafficlightPB);
            #endregion

            #region ClickEvents
            trafficlightPB.MouseDown += MouseDownEvent;
            trafficlightPB.MouseMove += MouseMoveEvent;
            trafficlightPB.MouseUp += MouseClickUp;
        }

        /// <summary>
        /// Method triggered when a mousebutton is pressed down.
        /// </summary>
        private void MouseDownEvent(object o, MouseEventArgs mea)
        {
            mouseDownPoint = new Point(mea.X / 100 * 100, mea.Y / 100 * 100);
            mouseMovePoint = mea.Location;
            if (oldselectedTile != null)
            {
                backgroundBC.AddObject(oldselectedTile.DrawImage(), oldselectedTile.position);
                oldselectedTile = null;
            }
            /// Remove a tile by clicking with the right mouse button
            if (mea.Button == System.Windows.Forms.MouseButtons.Right && simwindow.BovenSchermLinks.Simulation == false)
			{
				if (state == "selected")
				{
					selectedTile = null;
					removeTile(mea.Location);
				}
				else 
				{
					removeTile(mea.Location);
				}

			}


            /// Als je een weg wil bouwen
            else if (state == "building")
                DrawTile(mea.Location, currentBuildTile);

            /// Als je een route wil aanklikken voor een groene golf
            /// deze aanpassen, zodat het nummer overeenkomt met nummer voor het selecteren van de groene golf
            //else if (stateGreenWave == "buildingGreenWave")
                //DrawGreenWave(mea);
        }

        /// <summary>
        /// Method triggerd whenever the mouse is moving.
        /// </summary>
        private void MouseMoveEvent(object o, MouseEventArgs mea)
        {
            if (mouseDownPoint != new Point(0, 0))
            {
                /// Move the map
                if (state == "selected")
                    MoveMap(mea);
                if (simwindow.BovenSchermLinks.Simulation == false)
                {
                    /// Draws a line of straight roads on mousedown
                    if (Methods.TileIsStraight(this,mouseDownPoint, mea.Location) && state == "building" && simulation.simStarted == false && mea.Button == System.Windows.Forms.MouseButtons.Left)
                        DrawTile(mea.Location, currentBuildTile);
                    /// Erase all the tiles that you come across with your mouse
                    if (state == "eraser")
                        removeTile(mea.Location);
                }
            }
        }

        /// <summary>
        /// Method triggered whenever a mouse button goes up
        /// </summary>
        private void MouseClickUp(object obj, MouseEventArgs mea)
        {
            mouseDownPoint = new Point(0, 0); mouseMovePoint = new Point(0, 0);
            /// De eerder geselecteerde tile wordt opnieuw getekend en verwijdert zo de blauwe rand
            if (oldselectedTile != null)
            {
                backgroundBC.AddObject(oldselectedTile.DrawImage(), oldselectedTile.position);
                oldselectedTile = null;
            }

            if (isMoved == false)
            {
                /// Als de gum-tool is aangeklikt
                if (state == "eraser")
                {
                    removeTile(mea.Location);
                }
                /// Als de select-tool is aangeklikt
                else if (state == "selected")
                    DrawSelectLine(mea.Location);
            }
            isMoved = false;
        }
            #endregion
    }
}
