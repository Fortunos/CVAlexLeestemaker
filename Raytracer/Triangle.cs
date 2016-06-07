using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Template;

namespace template
{
    class Triangle : Primitive
    {
        private Vector3 normal, p, e1, e2;

        public Triangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 color)
        {
            p = p0;
            e1 = p1 - p0;
            e2 = p2 - p0;
            normal = Vector3.Cross(e1, e2).Normalized();
            this.color = color;
        }

        public override void Intersect(Ray ray)
        {
            float eps = 0.000001f;
            Vector3 P = Vector3.Cross(ray.D, e2);
            float det = Vector3.Dot(e1, P);

            if (det > -eps && det < eps)
            {
                return;
            }

            Vector3 T = ray.O - p;
            float inv_det = 1 / det;
            float u = Vector3.Dot(T, P) * inv_det;

            if (u < 0 || u > 1)
            {
                return;
            }

            Vector3 Q = Vector3.Cross(T, e1);
            float v = Vector3.Dot(ray.D, Q) * inv_det;

            if (v < 0 || u + v > 1)
            {
                return;
            }

            float t = Vector3.Dot(e2, Q) * inv_det;

            if (t > eps && t < ray.l)
            {
                ray.l = t;
                ray.objectHit = this;
            }
        }

        public virtual void DrawDebug(Surface screen)
        {
            int x1 = Debug.TX(p.X, screen.width);
            int z1 = Debug.TZ(p.Z, screen.height);
            int x2 = Debug.TX((e1 + p).X, screen.width);
            int z2 = Debug.TZ((e1 + p).Z, screen.height);
            int x3 = Debug.TX((e2 + p).X, screen.width);
            int z3 = Debug.TZ((e2 + p).Z, screen.height);

            screen.Line(x1, z1, x2, z2, Raytracer.VectorToColor(Vector3.One));
            screen.Line(x1, z1, x3, z3, Raytracer.VectorToColor(Vector3.One));
            screen.Line(x2, z2, x3, z3, Raytracer.VectorToColor(Vector3.One));
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return normal;
        }
    }
}
