using System.Drawing;
using OpenTK.Input;

namespace Template
{
    class Game
    {
        // member variables
        public Surface screen;
        public Camera camera;
        private Scene scene;

        // initialize
        public void Init(Rectangle bounds)
        {
            screen = new Surface(bounds.Width, bounds.Height);
            camera = new Camera(bounds.Width, bounds.Height);
            scene = new Scene();

            Application.CreateApplication(camera, screen, scene, bounds);
            Debug.CreateDebug(scene, screen);
        }

        // tick: renders one frame
        public void Tick()
        {
            //Call the application.HandleInput() and Raytrace() after clearing the screen.
            screen.Clear(0);
            Application.HandleInput();
            Application.Raytrace();

            //Draw the debug screen.
            Debug.DrawDebugScreen();
            Debug.Rays.Clear();
        }

        //Called on every mousemove event.
        public void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            Application.OnMouseMove(sender, e);
        }
    }
}