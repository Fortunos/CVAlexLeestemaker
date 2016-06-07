using System;
using OpenTK;

namespace Template
{
    class Sphere : Primitive
    {
        Vector3 position;
        float radius;

        public Sphere(Vector3 position, float radius, Vector3 color)
        {
            this.position = position;
            this.radius = radius;
            this.color = color;
        }

        //Code overgenomen uit slides
        public override void Intersect(Ray ray)
        {
            //possibly have to change this to incorporate intersecting the orb from inside
            Vector3 c = this.position - ray.O;
            float t = Vector3.Dot(c, ray.D);
            Vector3 q = c - t * ray.D;
            float p2 = Vector3.Dot(q, q);
            float r2 = radius * radius;
            if (p2 > r2) return;
            t -= (float)Math.Sqrt(r2 - p2);
            if ((t < ray.l) && (t > 0))
            {
                ray.l = t;
                ray.objectHit = this;
            }
            // or: ray.t = min( ray.t, max( 0, t ) );
        }

        public override void DrawDebug(Surface screen)
        {
            screen.Circle((radius / 10) * screen.width, Debug.TX(position.X, screen.width), Debug.TZ(position.Z, screen.height), Raytracer.VectorToColor(color), (float)100);
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return (point - position).Normalized();
        }
    }
}
