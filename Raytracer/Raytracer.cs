using System;
using System.Drawing.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template
{
    class Raytracer
    {
        private const float Epsilon = 0.00001f;
        private bool AntiAlias = false;

        public void Render(Camera cam, Surface screen, Scene scene)
        {
            Parallel.For(0, screen.height, y =>
            {
                float hi = y / (float)screen.height;

                for (int x = 0; x < (screen.width / 2); x++)
                {
                    float wi = x / ((float)screen.width / 2);

                    RenderPixel(cam, screen, scene, hi, wi, x, y);
                }
            });
        }

        public void RenderPixel(Camera cam, Surface screen, Scene scene, float hi, float wi, int x, int y)
        {
            Vector3[] points = cam.GetPoints;
            Vector3 direction = new Vector3();
            Vector3 result;
            Ray ray;

            if (AntiAlias)
            {

                float offX = 1 / 2f * 1 / screen.width;
                float offY = 1 / 2f * 1 / screen.height;

                Vector3 directionLB = points[0] + wi * (points[1] - points[0]) + hi * (points[2] - points[0]);
                Vector3 directionRB = points[0] + (wi + offX) * (points[1] - points[0]) + hi * (points[2] - points[0]);
                Vector3 directionLO = points[0] + wi * (points[1] - points[0]) + (hi + offY) * (points[2] - points[0]);
                Vector3 directionRO = points[0] + (wi + offX) * (points[1] - points[0]) +
                                      (hi + offY) * (points[2] - points[0]);

                ray = new Ray(cam.Position, directionLB, float.MaxValue);
                Ray ray2 = new Ray(cam.Position, directionRB, float.MaxValue);
                Ray ray3 = new Ray(cam.Position, directionLO, float.MaxValue);
                Ray ray4 = new Ray(cam.Position, directionRO, float.MaxValue);

                Vector3 res1 = CastPrimaryRay(ray, scene);
                Vector3 res2 = CastPrimaryRay(ray2, scene);
                Vector3 res3 = CastPrimaryRay(ray3, scene);
                Vector3 res4 = CastPrimaryRay(ray4, scene);

                result = (res1 + res2 + res3 + res4) / 4;
            }
            else
            {
                direction = points[0] + wi * (points[1] - points[0]) + hi * (points[2] - points[0]);

                ray = new Ray(cam.Position, direction, float.MaxValue);
                result = CastPrimaryRay(ray, scene);
            }

            if (ray.D.Y < .01f && ray.D.Y > -.01f && x % 10 == 0)
            {
                Debug.Rays.Add(ray);
            }

            //screen.pixels[x + y * screen.width] = VectorToColor(CastPrimaryRay(ray, screen, scene, 0));
            screen.pixels[x + y * screen.width] = VectorToColor(result);
        }

        private Vector3 CastPrimaryRay(Ray ray, Scene scene, int depth = 0)
        {
            foreach (Primitive p in scene.primitives)
            {
                p.Intersect(ray);
            }
            //Nu alleen nog de kleur van het object, geen shadow rays e.d.
            if (ray.objectHit == null) //If no object has been hit load the skybox.
            {
                return Vector3.Zero;
            }

            if (ray.D.Y < .01f && ray.D.Y > -.01f)
            {
                if (depth != 0)
                    Debug.Rays.Add(ray);
            }

            Vector3 intersection = ray.GetPoint();
            Vector3 color = ray.objectHit.GetTexture(intersection);

            if (ray.objectHit.specular && depth < 20)
            {
                //Object is een spiegel, cast een tweede primary ray
                Vector3 N = ray.objectHit.GetNormal(ray.GetPoint());
                Vector3 R = ray.D - 2 * (Vector3.Dot(ray.D, N) * N);
                Ray secondaryRay = new Ray(intersection, R, float.MaxValue);
                return color * CastPrimaryRay(secondaryRay, scene, depth + 1);
            }
            //Object weerkaatst licht diffuus, cast shadowrays naar lampen OF max depth is gehaald en meer primary rays tekenen wordt te duur
            return color * CastShadowRay(intersection, ray.objectHit, scene);
        }

        private Vector3 CastShadowRay(Vector3 intersection, Primitive prim, Scene scene)
        {
            Vector3 light = new Vector3(0, 0, 0);
            foreach (Light l in scene.lights)
            {
                //Richting van de ray is van lamp naar object
                Vector3 toLight = intersection - l.pos;
                float tmax = toLight.Length - 2 * Epsilon;
                Ray shadowRay = new Ray(l.pos - toLight * Epsilon, toLight, float.MaxValue);

                //Negatief, omdat de shadowray van lamp naar object gaat ipv andersom
                Vector3 N = -1f * prim.GetNormal(intersection);
                float incidence = Vector3.Dot(N, shadowRay.D);

                if (incidence > 0)
                {
                    foreach (Primitive p in scene.primitives)
                    {
                        p.Intersect(shadowRay);
                        //Als de ray iets raakt tussen de lamp en het object, abort mission
                        if (shadowRay.l < tmax)
                        {
                            break;
                        }
                    }
                    //Check of er geen objecten tussen de lamp en het object zitten
                    if (shadowRay.l >= tmax)
                    {
                        light += (l.color * (1 / (shadowRay.l * shadowRay.l))) * incidence;
                    }
                }
            }

            foreach (Spotlight spot in scene.spots)
            {
                //Richting van de ray is van lamp naar object
                Vector3 toLight = intersection - spot.pos;
                float tmax = toLight.Length - 2 * Epsilon;
                Ray shadowRay = new Ray(spot.pos - toLight * Epsilon, toLight, float.MaxValue);

                //Negatief, omdat de shadowray van lamp naar object gaat ipv andersom
                Vector3 N = -1f * prim.GetNormal(intersection);
                float incidence = Vector3.Dot(N, shadowRay.D);
                float incidenceOnSpot = Vector3.Dot(shadowRay.D, spot.D);

                if (incidence > 0 && incidenceOnSpot > spot.minDot)
                {
                    foreach (Primitive p in scene.primitives)
                    {
                        p.Intersect(shadowRay);
                        //Als de ray iets raakt tussen de lamp en het object, abort mission
                        if (shadowRay.l < tmax)
                        {
                            break;
                        }
                    }
                    //Check of er geen objecten tussen de lamp en het object zitten
                    if (shadowRay.l >= tmax)
                    {
                        light += (spot.color * (1 / (shadowRay.l * shadowRay.l))) * incidence;
                    }
                }
            }

            return light;
        }

        public static int VectorToColor(Vector3 input)
        {
            int[] values = new int[3];
            values[0] = Math.Min((int)(input.X * 255), 255);
            values[1] = Math.Min((int)(input.Y * 255), 255);
            values[2] = Math.Min((int)(input.Z * 255), 255);
            return (values[0] << 16) + (values[1] << 8) + values[2];
        }
    }
}
