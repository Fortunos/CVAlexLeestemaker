using System;
using OpenTK;

namespace Template
{
    class Plane : Primitive
    {
        private Vector3 normal;
        private float distanceToOrigin;

        //Plane constructor, takes a normal Vector, a distance to origin and a color Vector
        public Plane(Vector3 normal, float distanceToOrigin, Vector3 color)
        {
            this.normal = normal.Normalized();
            this.distanceToOrigin = distanceToOrigin;
            u = Perpendicular(normal);
            v = Vector3.Cross(normal, u).Normalized();
            this.color = color;
        }

        //The intersection of a plane and a ray.
        public override void Intersect(Ray ray)
        {
            //Plane: p * N + d = 0
            //Ray:p(t) = O + tD
            //Intersect => substitute and solve

            float result = -(Vector3.Dot(ray.O, normal) + distanceToOrigin) / (Vector3.Dot(ray.D, normal));
            if (result > 0 && result < ray.l)
            {
                //Dit object staat dichter bij de camera dan een eerder geraakt object
                ray.l = result;
                ray.objectHit = this;
            }
        }

        public override Vector3 GetNormal(Vector3 point)
        {
            return normal;
        }

        public override Vector3 GetTexture(Vector3 point)
        {
            if (textured)
            {
                float delta1 = Vector3.Dot(point, u);
                float delta2 = Vector3.Dot(point, v);

                return (((int) (2 * delta1) + (int) (2 * delta2)) & 1) * color;
            }
            return color;
        }
    }
}
