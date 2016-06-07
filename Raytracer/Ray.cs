using OpenTK;

namespace Template
{
    public class Ray
    {
        public Vector3 O, D; //origin and direction.
        public float l; //The length
        public Primitive objectHit; //The primitive the ray hits (if applicable).

        public Ray(Vector3 Origin, Vector3 Direction, float length)
        {
            O = Origin;
            D = Direction.Normalized();
            l = length;
        }

        //The vector at which the intersection was.
        public Vector3 GetPoint()
        {
            return O + l * D;
        }

        //Draw the ray on the debug.
        public void DrawRay(Surface screen)
        {
            int x1 = Debug.TX(O.X, screen.width);
            int y1 = Debug.TZ(O.Z, screen.height);
            int x2 = Debug.TX((O + D * l).X, screen.width);
            int y2 = Debug.TZ((O + D * l).Z, screen.height);

            screen.Line(x1, y1, x2, y2, 16776960);
        }
    }
}
