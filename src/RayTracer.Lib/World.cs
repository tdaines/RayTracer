using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Lib
{
    public class World
    {
        public List<PointLight> Lights { get; }
        public List<Shape> Objects { get; }

        public World()
        {
        }

        public World(PointLight light, List<Shape> objects)
        {
            Lights = new List<PointLight>(1) { light };
            Objects = objects;
        }

        public static World DefaultWorld()
        {
            var light = new PointLight(new Point(-10, 10, -10), Color.White);

            return DefaultWorld(light);
        }
        
        public static World DefaultWorld(PointLight light)
        {
            var spheres = new List<Shape>
            {
                new Sphere(new Material(new Color(0.8f, 1.0f, 0.6f), diffuse: 0.7f, specular: 0.2f)),
                new Sphere(Matrix4x4.Scaling(0.5f, 0.5f, 0.5f))
            };
            
            return new World(light, spheres);
        }

        public Intersections Intersect(Ray ray)
        {
            List<Intersection> allIntersections = new List<Intersection>();

            for (int i = 0; i < Objects.Count; i++)
            {
                var intersections = ray.Intersect((Sphere)Objects[i]);
                allIntersections.AddRange(intersections.Where(xs => xs.Time >= 0));
            }
            
            return new Intersections(allIntersections);
        }

        public Color ShadeHit(IntersectionInfo intersectionInfo)
        {
            var material = intersectionInfo.Intersection.Object.Material;
            var point = intersectionInfo.Point;
            var eye = intersectionInfo.EyeVector;
            var normal = intersectionInfo.Normal;
            
            var color = new Color(0, 0, 0);

            for (int i = 0; i < Lights.Count; i++)
            {
                var light = Lights[i];
                color += light.Lighting(material, point, eye, normal);
            }

            return color;
        }

        public Color ColorAt(Ray ray)
        {
            var intersections = Intersect(ray);
            var intersection = intersections.Hit();

            if (intersection == null)
            {
                return Color.Black;
            }
            
            var intersectionInfo = new IntersectionInfo(intersection, ray);

            return ShadeHit(intersectionInfo);
        }

        public Canvas Render(Camera camera)
        {
            var canvas = new Canvas(camera.Width, camera.Height);

            for (int y = 0; y < camera.Height; y++)
            {
                for (int x = 0; x < camera.Width; x++)
                {
                    var ray = camera.RayForPixel(x, y);
                    var color = ColorAt(ray);
                    canvas[x, y] = color;
                }
            }

            return canvas;
        }
    }
}