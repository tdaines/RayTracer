using System;

namespace RayTracer.Lib.Shapes
{
    public class Sphere : Shape
    {
        public Sphere()
        {
        }

        public Sphere(Matrix4x4 transform) : base(transform)
        {
        }

        public Sphere(Material material) : base(material)
        {
        }

        public Sphere(Matrix4x4 transform, Material material) : base(transform, material)
        {
        }

        protected override Vector ObjectNormal(Point objectPoint)
        {
            return objectPoint - Point.Zero;
        }

        protected override Intersections IntersectTransformedRay(Ray ray)
        {
            Vector sphereToRay = ray.Origin - Point.Zero;

            var a = Vector.Dot(ray.Direction, ray.Direction);
            var b = 2 * Vector.Dot(ray.Direction, sphereToRay);
            var c = Vector.Dot(sphereToRay, sphereToRay) - 1;

            var discriminant = (b * b) - (4 * a * c);

            if (discriminant < 0)
            {
                return new Intersections();
            }

            var time1 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
            var time2 = (-b + MathF.Sqrt(discriminant)) / (2 * a);

            var min = MathF.Min(time1, time2);
            var max = MathF.Max(time1, time2);
            
            return new Intersections(new Intersection(min, this), new Intersection(max, this));
        }
    }
}