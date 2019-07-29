using System;

namespace RayTracer.Lib
{
    public class Ray
    {
        public Point Origin { get; }
        public Vector Direction { get; private set; }

        public Ray(Point origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Point Position(float time)
        {
            return Origin + (Direction * time);
        }

        public Intersections Intersect(Sphere sphere)
        {
            var inverseTransform = Matrix4x4.Inverse(sphere.Transform);
            var ray = Transform(this, inverseTransform);
            
            Vector sphereToRay = ray.Origin - new Point(0, 0, 0);

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
            
            return new Intersections(new Intersection(min, sphere), new Intersection(max, sphere));
        }

        public static Ray Transform(Ray ray, Matrix4x4 transformation)
        {
            return new Ray(
                ray.Origin * transformation,
                ray.Direction * transformation);
        }
    }
}