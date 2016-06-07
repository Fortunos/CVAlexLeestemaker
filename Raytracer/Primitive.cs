using OpenTK;

namespace Template
{
    public abstract class Primitive
    {
        /*
         * Baseclass of all primitives.
         */
        public Vector3 color, u, v;
        public bool specular, textured;

        //All virtual methods are implemented in the children classes with override methods.
        public virtual void Intersect(Ray ray) //For the intersection.
        {

        }

        public virtual void DrawDebug(Surface screen) //For drawing on the debug screen.
        {

        }

        public virtual Vector3 GetNormal(Vector3 point) //Returns the normal.
        {
            return new Vector3();
        }

        public virtual Vector3 GetTexture(Vector3 point) //Returns the texture.
        {
            return color;
        }

        public static Vector3 Perpendicular(Vector3 input)
        {
            return new Vector3(input.Y, -input.X, 0).Normalized();
        }
    }
}
