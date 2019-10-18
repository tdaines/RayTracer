using System;
using System.Collections.Generic;
using RayTracer.Lib.Shapes;

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

        public void Add(PointLight light)
        {
            Lights.Add(light);
        }

        public void Add(Shape shape)
        {
            Shapes.Add(shape);
        }

        public Intersections Intersect(Ray ray, bool shadowIntersections = false)
        {
            List<Intersection> allIntersections = new List<Intersection>();

            for (int i = 0; i < Shapes.Count; i++)
            {
                var shape = Shapes[i];
                if (!shadowIntersections || shape.CastsShadow)
                {
                    var intersections = shape.Intersect(ray);
                    allIntersections.AddRange(intersections);
                }
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

        public Canvas Render(Camera camera, int recursiveDepth = 5, int raysPerPixel = 1)
        {
            var canvas = new Canvas(camera.Width, camera.Height);
            
            for (int y = 0; y < camera.Height; y++)
            {
                for (int x = 0; x < camera.Width; x++)
                {
                    canvas[x, y] = ColorAt(camera, x, y, recursiveDepth, raysPerPixel);
                }
            }

            return canvas;
        }

        private Color ColorAt(Camera camera, int x, int y, int recursiveDepth, int raysPerPixel)
        {
            if (raysPerPixel == 1)
            {
                var ray = camera.RayForPixel(x, y);
                return ColorAt(ray, recursiveDepth);
            }
            
            int rows = (int)MathF.Sqrt(raysPerPixel);
            int cols = rows;
            var offset = 1 / (MathF.Sqrt(raysPerPixel) * 2);
            var step = 2 * offset;
            var color = Color.Black;
                        
            for (int row = 0; row < rows; row++)
            {
                var yOffset = offset + row * step;
                    
                for (int col = 0; col < cols; col++)
                {
                    var xOffset = offset + col * step;
                        
                    var ray = camera.RayForPixel(x, y, xOffset, yOffset);
                    color += ColorAt(ray, recursiveDepth);
                }
            }
                
            var red = color.R / raysPerPixel;
            var green = color.G / raysPerPixel;
            var blue = color.B / raysPerPixel;
                
            return new Color(red, green, blue);
        }

        public bool IsShadowed(Point point, PointLight light)
        {
            var vectorToLight = light.Position - point;
            var directionToLight = Vector.Normalize(vectorToLight);
            
            var rayToLight = new Ray(point, directionToLight);
            var intersections = Intersect(rayToLight, true);

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