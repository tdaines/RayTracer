using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Add(PointLight light)
        {
            Lights.Add(light);
        }

        public void Add(Shape shape)
        {
            Shapes.Add(shape);
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

        public Color ShadeHit(IntersectionInfo intersectionInfo, int remaining)
        {
            var material = intersectionInfo.Intersection.Shape.Material;
            var shape = intersectionInfo.Intersection.Shape;
            var eye = intersectionInfo.EyeVector;
            var normal = intersectionInfo.Normal;
            var overPoint = intersectionInfo.OverPoint;

            var color = Color.Black;

            for (int i = 0; i < Lights.Count; i++)
            {
                var light = Lights[i];
                var inShadow = IsShadowed(overPoint, light);

                var surfaceColor = light.Lighting(material, shape, overPoint, eye, normal, inShadow);
                var reflectedColor = ReflectedColor(intersectionInfo, remaining);
                var refractedColor = RefractedColor(intersectionInfo, remaining);

                if (material.Reflective > 0 && material.Transparency > 0)
                {
                    var reflectance = intersectionInfo.Reflectance();
                    color += surfaceColor 
                           + reflectedColor * reflectance
                           + refractedColor * (1- reflectance);
                }
                else
                {
                    color += surfaceColor + reflectedColor + refractedColor;
                }
            }

            return color;
        }

        public Color ColorAt(Ray ray, int remaining)
        {
            var intersections = Intersect(ray);
            var intersection = intersections.Hit();

            if (intersection == null)
            {
                return Color.Black;
            }
            
            var intersectionInfo = new IntersectionInfo(intersections, intersection, ray);

            return ShadeHit(intersectionInfo, remaining);
        }

        public Canvas Render(Camera camera, int recursiveDepth = 5)
        {
            var canvas = new Canvas(camera.Width, camera.Height);

            for (int y = 0; y < camera.Height; y++)
            {
                for (int x = 0; x < camera.Width; x++)
                {
                    var ray = camera.RayForPixel(x, y);
                    var color = ColorAt(ray, recursiveDepth);
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

        public Color ReflectedColor(IntersectionInfo info, int remaining)
        {
            if (remaining < 1)
            {
                return Color.Black;
            }
            
            var reflective = info.Intersection.Shape.Material.Reflective;
            
            if (reflective.ApproximatelyEquals(0))
            {
                return Color.Black;
            }
            
            var reflectedRay = new Ray(info.OverPoint, info.ReflectVector);
            var color = ColorAt(reflectedRay, remaining - 1);

            return color * reflective;
        }

        public Color RefractedColor(IntersectionInfo info, int remaining)
        {
            if (remaining < 1)
            {
                return Color.Black;
            }
            
            float materialTransparency = info.Intersection.Shape.Material.Transparency;
            if (materialTransparency.ApproximatelyEquals(0))
            {
                return Color.Black;
            }
            
            // Find the ratio of the first index of refraction to the second
            float ratio = info.RefractiveIndex1 / info.RefractiveIndex2;

            // Theta_i is the angle of the incoming ray
            // Cos(Theta_i) is the same as the dot product of the two vectors
            float cosineI = Vector.Dot(info.EyeVector, info.Normal);
            
            // Theta_t is the angle of the refracted ray
            // Find Sin(Theta_t)^2
            float sin2T = (ratio * ratio) * (1 - (cosineI * cosineI));

            if (sin2T > 1)
            {
                // Total internal reflection
                return Color.Black;
            }

            float cosineT = MathF.Sqrt(1 - sin2T);
            
            // Compute direction of the refracted ray
            var direction = info.Normal * (ratio * cosineI - cosineT) - info.EyeVector * ratio;
            
            var refractedRay = new Ray(info.UnderPoint, direction);

            return ColorAt(refractedRay, remaining - 1) * materialTransparency;
        }
    }
}