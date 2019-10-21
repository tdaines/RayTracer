using System;
using System.Collections.Generic;

namespace RayTracer.Lib.Shapes
{
    public class Cylinder : Shape
    {
        public float Min { get; set; } = float.NegativeInfinity;
        public float Max { get; set; } = float.PositiveInfinity;
        public bool Capped { get; set; } = false;
        
        public Cylinder()
        {
        }

        public Cylinder(Matrix4x4 transform) : base(transform)
        {
        }

        public Cylinder(Material material) : base(material)
        {
        }

        public Cylinder(Matrix4x4 transform, Material material) : base(transform, material)
        {
        }

        protected override Vector ObjectNormal(Point objectPoint)
        {
            // Compute the square of the distance from the y-axis
            var distance = MathF.Pow(objectPoint.X, 2) + MathF.Pow(objectPoint.Z, 2);

            var y = objectPoint.Y;

            if (distance < 1)
            {
                if (y > Max || y.ApproximatelyEquals(Max))
                {
                    return Vector.UnitY;
                }

                if (y < Min || y.ApproximatelyEquals(Min))
                {
                    return -Vector.UnitY;
                }
            }
            
            return new Vector(objectPoint.X, 0, objectPoint.Z);
        }

        protected override Intersections IntersectTransformedRay(Ray ray)
        {
            var a = MathF.Pow(ray.Direction.X, 2) + MathF.Pow(ray.Direction.Z, 2);

            var intersections = new List<Intersection>(2);
            
            if (a.ApproximatelyEquals(0))
            {
                // ray is parallel to y-axis
                IntersectCaps(ray, intersections);
                return new Intersections(intersections);
            }
            
            var b = 2 * ray.Origin.X * ray.Direction.X
                  + 2 * ray.Origin.Z * ray.Direction.Z;
            
            var c = MathF.Pow(ray.Origin.X, 2) + MathF.Pow(ray.Origin.Z, 2) - 1;

            var discriminant = MathF.Pow(b, 2) - 4 * a * c;

            if (discriminant < 0)
            {
                // ray does not intersect cylinder
                return new Intersections();
            }

            var t0 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
            var t1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);

            if (t0 > t1)
            {
                // swap
                (t0, t1) = (t1, t0);
            }

            var y = ray.Origin.Y + t0 * ray.Direction.Y;
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
            if (CheckCap(ray, t))
            {
                intersections.Add(new Intersection(t, this));
            }
            
            // Check for an intersection with the upper end cap
            // by intersecting the ray with the plane at y = Max
            t = (Max - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, t))
            {
                intersections.Add(new Intersection(t, this));
            }
        }

        private bool CheckCap(Ray ray, float time)
        {
            var x = ray.Origin.X + time * ray.Direction.X;
            var z = ray.Origin.Z + time * ray.Direction.Z;

            var sum = MathF.Pow(x, 2) + MathF.Pow(z, 2);

            return sum < 1 || sum.ApproximatelyEquals(1);
        }
    }
}