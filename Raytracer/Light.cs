using OpenTK;

namespace Template
{
    /*
     * Class for creating the light sources and the spotlights.
     */
    public class Light : Primitive
    {
        public Vector3 pos;

        //Light constructor, takes a position Vector and a color Vector.
        public Light(Vector3 position, Vector3 color)
        {
            pos = position;
            this.color = color;
        }
    }

    public class Spotlight : Primitive
    {
        public Vector3 pos, D;
        public float minDot;

        //Spotlight constructor, takes a position Vector, a color Vector, a direction Vector and 
        public Spotlight(Vector3 position, Vector3 color, Vector3 direction, float minDot)
        {
            pos = position;
            this.color = color;
            D = direction.Normalized();
            this.minDot = minDot;
        }
    }
}