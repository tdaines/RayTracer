using System.Collections.Generic;
using System.Linq;
using RayTracer.Lib.Patterns;

namespace RayTracer.Lib
{
    public class World
    {
        public List<PointLight> Lights { get; }
        public List<Shape> Shapes { get; }

        public World()
        {
            Lights = new List<PointLight>();
            Shapes = new List<Shape>();
        }

        public static World DefaultWorld()
        {
            var light = new PointLight(new Point(-10, 10, -10), Color.White);

            return DefaultWorld(light);
        }
        
        public static World DefaultWorld(PointLight light)
        {
            var world = new World();
            
            world.Lights.Add(light);
            world.Shapes.Add(new Sphere(new Material(new Color(0.8f, 1.0f, 0.6f), diffuse: 0.7f, specular: 0.2f)));
            world.Shapes.Add(new Sphere(Matrix4x4.Scaling(0.5f, 0.5f, 0.5f)));

            return world;
        }

        public Intersections Intersect(Ray ray)
        {
            List<Intersection> allIntersections = new List<Intersection>();

            for (int i = 0; i < Shapes.Count; i++)
            {
                var intersections = Shapes[i].Intersect(ray);
                allIntersections.AddRange(intersections.Where(xs => xs.Time >= 0));
            }
            
            return new Intersections(allIntersections);
        }

        public Color ShadeHit(IntersectionInfo intersectionInfo)
        {
            var material = intersectionInfo.Intersection.Shape.Material;
            var shape = intersectionInfo.Intersection.Shape;
            var point = intersectionInfo.Point;
            var eye = intersectionInfo.EyeVector;
            var normal = intersectionInfo.Normal;
            var overPoint = intersectionInfo.OverPoint;

            var color = Color.Black;

            for (int i = 0; i < Lights.Count; i++)
            {
                var light = Lights[i];
                var inShadow = IsShadowed(overPoint, light);
                color += light.Lighting(material, shape, overPoint, eye, normal, inShadow);
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

        public bool IsShadowed(Point point, PointLight light)
        {
            var vectorToLight = light.Position - point;
            var directionToLight = Vector.Normalize(vectorToLight);
            
            var rayToLight = new Ray(point, directionToLight);
            var intersections = Intersect(rayToLight);

            // Get closest intersection
            var hit = intersections.Hit();
            
            // If hit occurred between point and light,
            // then point is in shadow
            if (hit != null)
            {
                var distanceToLight = vectorToLight.Length();
                if (hit.Time < distanceToLight)
                {
                    return true;
                }
            }

            return false;
        }
    }
}