using System;

namespace RayTracer.Lib
{
    public class Plane : Shape
    {
        // Normal on xz plane always points up in y direction
        private readonly Vector normal = new Vector(0, 1, 0);
        
        public Plane()
        {
        }

        public Plane(Matrix4x4 transform) : base(transform)
        {
        }

        public Plane(Material material) : base(material)
        {
        }

        public Plane(Matrix4x4 transform, Material material) : base(transform, material)
        {
        }

        protected override Vector ObjectNormal(Point objectPoint)
        {
            return normal;
        }

        protected override Intersections IntersectTransformedRay(Ray ray)
        {
            if (ray.Direction.Y.ApproximatelyEquals(0f))
            {
                return new Intersections();
            }

            var time = -ray.Origin.Y / ray.Direction.Y;
            
            return new Intersections(new Intersection(time, this));
        }
    }
}