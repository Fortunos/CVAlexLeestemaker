using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;

namespace Template
{
    public class Application
    {
        /*
         * This class is responcible for taking care of user input and the calling of the raytracer.
         * The needed variables.
         */
        private static Camera camera;
        private static Surface screen;
        private static Scene scene;
        private static Raytracer raytracer;

        //Variables for the mouse movement.
        private static Vector2 mouseDelta;
        private static Rectangle bounds;
        private static Point currentCursor;
        private static Point previousCursor;

        //Constructor of the application.
        static public void CreateApplication(Camera cam, Surface scr, Scene sce, Rectangle bound)
        {
            raytracer = new Raytracer();
            camera = cam;
            screen = scr;
            scene = sce;
            bounds = bound;

            //Move the cursor to the middle of the screen and instantiate the variables to measure mouse movement.
            Cursor.Position = new Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
            currentCursor = Cursor.Position;
            previousCursor = Cursor.Position;

            mouseDelta = Vector2.Zero;
        }

        //This function handles all of the user input.
        static public void HandleInput()
        {
            if (currentCursor != previousCursor) //If the mouse has been moved true is passed.
            {
                camera.HandleInput(true, mouseDelta);
                previousCursor = currentCursor;
                mouseDelta = Vector2.Zero;
            }
            else
            {
                camera.HandleInput(false, Vector2.Zero); //If the mouse has not been moved.
            }
        }

        //Called on every mousemove event.
        public static void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            mouseDelta = new Vector2(e.XDelta, e.YDelta);
            currentCursor = Cursor.Position;
        }

        //The raytracer.
        static public void Raytrace()
        {
            raytracer.Render(camera, screen, scene);
        }
    }
}