using System;
using System.Collections.Generic;
using RayTracer.Lib.Shapes;

namespace RayTracer.Lib
{
    public class IntersectionInfo
    {
        public Intersections Intersections { get; }
        public Intersection Intersection { get; }
        public Point Point { get; }
        public Point OverPoint { get; }
        public Point UnderPoint { get; }
        public Vector EyeVector { get; }
        public Vector Normal { get; }
        public Vector ReflectVector { get; }
        public bool Inside { get; }
        public float RefractiveIndex1 { get; private set; } // n1
        public float RefractiveIndex2 { get; private set; } // n2

        private const float EPSILON = 0.001f;

        public IntersectionInfo(Intersections intersections, Intersection intersection, Ray ray)
        {
            Intersections = intersections;
            Intersection = intersection;
            Point = ray.Position(intersection.Time);
            EyeVector = -ray.Direction;
            Normal = intersection.Shape.Normal(Point);
            Inside = false;

            if (Vector.Dot(Normal, EyeVector) < 0)
            {
                Inside = true;
                Normal = -Normal;
            }

            OverPoint = Point + Normal * EPSILON;
            UnderPoint = Point - Normal * EPSILON;
            ReflectVector = Vector.Reflect(ray.Direction, Normal);
            
            CalculateRefractiveIndices();
        }

        private void CalculateRefractiveIndices()
        {
            var shapes = new List<Shape>();

            foreach (var intersection in Intersections)
            {
                if (intersection == Intersection)
                {
                    if (shapes.Count == 0)
                    {
                        RefractiveIndex1 = 1;
                    }
                    else
                    {
                        RefractiveIndex1 = shapes[^1].Material.RefractiveIndex;
                    }
                }

                if (shapes.Contains(intersection.Shape))
                {
                    shapes.Remove(intersection.Shape);
                }
                else
                {
                    shapes.Add(intersection.Shape);
                }

                if (intersection == Intersection)
                {
                    if (shapes.Count == 0)
                    {
                        RefractiveIndex2 = 1;
                    }
                    else
                    {
                        RefractiveIndex2 = shapes[^1].Material.RefractiveIndex;
                    }

                    break;
                }
            }
        }

        public float Reflectance()
        {
            // Use Schlick approximation
            
            // Find the cosine of the angle between the eye and normal vectors
            float cos = Vector.Dot(EyeVector, Normal);
            
            // Total internal reflection can only occur if n1 > n2
            if (RefractiveIndex1 > RefractiveIndex2)
            {
                var ratio = RefractiveIndex1 / RefractiveIndex2;
                var sin2T = (ratio * ratio) * (1 - (cos * cos));
                if (sin2T > 1)
                {
                    return 1;
                }

                cos = MathF.Sqrt(1 - sin2T);
            }

            var r0 = MathF.Pow((RefractiveIndex1 - RefractiveIndex2) / (RefractiveIndex1 + RefractiveIndex2), 2);
            return r0 + (1 - r0) * MathF.Pow(1 - cos, 5);
        }
    }
}