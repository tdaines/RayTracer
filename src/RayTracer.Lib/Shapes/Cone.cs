using System;
using System.Collections.Generic;

namespace RayTracer.Lib.Shapes
{
    public class Cone : Shape
    {
        public float Min { get; set; } = float.NegativeInfinity;
        public float Max { get; set; } = float.PositiveInfinity;
        public bool Capped { get; set; }
        
        public Cone()
        {
        }

        public Cone(Matrix4x4 transform) : base(transform)
        {
        }

        public Cone(Material material) : base(material)
        {
        }

        public Cone(Matrix4x4 transform, Material material) : base(transform, material)
        {
        }

        protected override Vector ObjectNormal(Point objectPoint)
        {
            var p = objectPoint;
            
            // Compute the square of the distance from the y-axis
            var distance = MathF.Pow(p.X, 2) + MathF.Pow(p.Z, 2);

            if (distance < 1)
            {
                if (p.Y > Max || p.Y.ApproximatelyEquals(Max))
                {
                    return Vector.UnitY;
                }

                if (p.Y < Min || p.Y.ApproximatelyEquals(Min))
                {
                    return -Vector.UnitY;
                }
            }

            var y = MathF.Sqrt(distance);
            if (p.Y > 0)
            {
                y = -y;
            }
            
            return new Vector(p.X, y, p.Z);
        }

        protected override Intersections IntersectTransformedRay(Ray ray)
        {
            var intersections = new List<Intersection>(2);
            
            var dx = ray.Direction.X;
            var dy = ray.Direction.Y;
            var dz = ray.Direction.Z;

            var ox = ray.Origin.X;
            var oy = ray.Origin.Y;
            var oz = ray.Origin.Z;

            float y;
            
            var a = MathF.Pow(dx, 2) - MathF.Pow(dy, 2) + MathF.Pow(dz, 2);
            var b = (2 * ox * dx) - (2 * oy * dy) + (2 * oz * dz);
            var c = MathF.Pow(ox, 2) - MathF.Pow(oy, 2) + MathF.Pow(oz, 2);

            if (a.ApproximatelyEquals(0))
            {
                if (b.ApproximatelyEquals(0))
                {
                    // ray misses
                    return new Intersections();
                }

                // ray is parallel to one of the cone's halves
                var t = -c / (2 * b);
                y = ray.Origin.Y + t * ray.Direction.Y;
                
                if (Min < y && y < Max)
                {
                    intersections.Add(new Intersection(t, this));
                }
                
                IntersectCaps(ray, intersections);
                
                return new Intersections(intersections);
            }

            var discriminant = MathF.Pow(b, 2) - 4 * a * c;
            if (discriminant.ApproximatelyEquals(0))
            {
                discriminant = 0;
            }
            
            if (discriminant < 0)
            {
                // ray does not intersect cone
                return new Intersections();
            }

            var t0 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
            var t1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);

            if (t0 > t1)
            {
                // swap
                (t0, t1) = (t1, t0);
            }

            y = ray.Origin.Y + t0 * ray.Direction.Y;
            if (Min < y && y < Max)
            {
                intersections.Add(new Intersection(t0, this));
            }
            
            y = ray.Origin.Y + t1 * ray.Direction.Y;
            if (Min < y && y < Max)
            {
                intersections.Add(new Intersection(t1, this));
            }
            
            IntersectCaps(ray, intersections);
            
            return new Intersections(intersections);
        }

        private void IntersectCaps(Ray ray, List<Intersection> intersections)
        {
            if (!Capped || ray.Direction.Y.ApproximatelyEquals(0))
            {
                return;
            }
            
            // Check for an intersection with the lower end cap
            // by intersecting the ray with the plane at y = Min
            var t = (Min - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t, MathF.Abs(Min)))
            {
                intersections.Add(new Intersection(t, this));
            }
            
            // Check for an intersection with the upper end cap
            // by intersecting the ray with the plane at y = Max
            t = (Max - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t, MathF.Abs(Max)))
            {
                intersections.Add(new Intersection(t, this));
            }
        }

        private bool CheckCap(Ray ray, float time, float radius)
        {
            var x = ray.Origin.X + time * ray.Direction.X;
            var z = ray.Origin.Z + time * ray.Direction.Z;

            var sum = MathF.Pow(x, 2) + MathF.Pow(z, 2);
            radius = MathF.Abs(radius);

            return sum < radius || sum.ApproximatelyEquals(radius);
        }
    }
}