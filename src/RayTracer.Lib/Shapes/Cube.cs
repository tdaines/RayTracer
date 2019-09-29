using System;

namespace RayTracer.Lib.Shapes
{
    public class Cube : Shape
    {
        private const float EPSILON = 0.0001f;
        
        public Cube()
        {
        }

        public Cube(Matrix4x4 transform) : base(transform)
        {
        }

        public Cube(Material material) : base(material)
        {
        }

        public Cube(Matrix4x4 transform, Material material) : base(transform, material)
        {
        }

        protected override Vector ObjectNormal(Point objectPoint)
        {
            throw new System.NotImplementedException();
        }

        protected override Intersections IntersectTransformedRay(Ray ray)
        {
            var (xMin, xMax) = CheckAxis(ray.Origin.X, ray.Direction.X);
            var (yMin, yMax) = CheckAxis(ray.Origin.Y, ray.Direction.Y);
            var (zMin, zMax) = CheckAxis(ray.Origin.Z, ray.Direction.Z);

            var min = MathF.Max(MathF.Max(xMin, yMin), zMin);
            var max = MathF.Min(MathF.Min(xMax, yMax), zMax);
            
            return new Intersections(new Intersection(min, this), new Intersection(max, this));
        }

        private (float min, float max) CheckAxis(float origin, float direction)
        {
            var minNumerator = -1 - origin;
            var maxNumerator = 1 - origin;

            var min = 0f;
            var max = 0f;
            
            if (MathF.Abs(direction) >= EPSILON)
            {
                min = minNumerator / direction;
                max = maxNumerator / direction;
            }
            else
            {
                min = minNumerator * float.PositiveInfinity;
                max = maxNumerator * float.PositiveInfinity;
            }

            if (min > max)
            {
                var temp = min;
                min = max;
                max = temp;
            }

            return (min, max);
        }
    }
}