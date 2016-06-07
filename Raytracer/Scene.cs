using System;
using System.Collections.Generic;
using Win32FileIO;
using OpenTK;
using template;

namespace Template
{
    public class Scene
    {
        private List<Material> materials;
        public List<Primitive> primitives = new List<Primitive>();
        public List<Light> lights = new List<Light>();
        public List<Spotlight> spots = new List<Spotlight>();

        public Scene()
        {
            //Het aanmaken van de scene
            //Drie spheres
            primitives.Add(new Sphere(new Vector3(-2, 0, 2.2f), 1, new Vector3(1, 0, 0)));
            primitives.Add(new Sphere(new Vector3(0, 0, 3), 1, new Vector3(.7f, .7f, .7f)));
            primitives.Add(new Sphere(new Vector3(2, 0, 3.8f), 1, new Vector3(0, 0, 1)));
            //Plane als ondergrond
            primitives.Add(new Plane(new Vector3(0, 1, 0), 2, new Vector3(1, 1, 1)));
            primitives[3].textured = true;
            //Spiegeltje spiegeltje aan de wand
            primitives[1].specular = true;
            Vector3 p0 = new Vector3(3, 0, 3f);
            Vector3 p1 = new Vector3(3.5f, 2, 3);
            Vector3 p2 = new Vector3(4, 0, 3);
            Vector3 p3 = new Vector3(5, 2, 2);
            primitives.Add(new Triangle(p0, p1, p2, new Vector3(1, 1, 1)));
            primitives.Add(new Triangle(p1, p3, p2, new Vector3(.8f, .2f, .5f)));
            primitives[4].specular = true;

            //Lamp
            lights.Add(new Light(new Vector3(0, 4, 3), new Vector3(20, 20, 20)));
            lights.Add(new Light(new Vector3(4, 3, 2), new Vector3(10, 10, 10)));

            //Spotlights
            spots.Add(new Spotlight(new Vector3(-2, 2, 5), new Vector3(10, 10, 10), new Vector3(0, -1, -1), 0.8f));
            spots.Add(new Spotlight(new Vector3(2, 2, 5), new Vector3(10, 10, 10), new Vector3(0, -1, -1), 0.8f));
        }
    }
}
