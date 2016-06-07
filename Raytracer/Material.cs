using OpenTK;

namespace Template
{
    /*
     * Class for creating the materials.
     */
    public class Material
    {
        private Vector3 color;
        private float diffuse;
        private float reflection;
        private float recursionDepth;

        //The material constructor, takes a color Vector, a diffuse value, reflection value and a recursionDepth.
        public Material(Vector3 color, float diffuse, float reflection, float recursionDepth)
        {
            this.color = color;
            this.diffuse = diffuse;
            this.reflection = reflection;
            this.recursionDepth = recursionDepth;
        }
    }
}
