using System.Collections.Generic;

namespace Template
{
    public class Debug
    {
        /*
         * The class for creating the debug screen.
         */
        public static List<Ray> Rays;

        private static Surface screen;
        private static Scene scene;

        //Constructor.
        public static void CreateDebug(Scene sce, Surface scr)
        {
            Rays = new List<Ray>();
            scene = sce;
            screen = scr;
        }

        //This function draws one in evrey ten rays on the screen.
        static public void DrawDebugScreen()
        {
            foreach(Primitive prim in scene.primitives)
            {
                prim.DrawDebug(screen);
            }

            foreach (Ray ray in Rays)
            {
                ray.DrawRay(screen);
            }
        }

        //Returns x position relative to the screen, can be given an offset from the centre.
        public static int TX(float x, int offSet)
        {
            return (int)((x / 10) * (screen.width / 2) + (3 * offSet / 4) - 1);
        }

        //Returns y position relative to the screen, can be given an offset from the centre.
        public static int TZ(float y, int offSet)
        {
            //return (int)((y / 10) * screen.height + (offSet / 2) - 1);
            return (int)(screen.height - ((y / 10) * screen.height + (offSet / 2) - 1));
        }
    }
}
