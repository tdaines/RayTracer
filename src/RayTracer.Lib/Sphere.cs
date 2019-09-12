namespace RayTracer.Lib
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

        public override Vector Normal(Point point)
        {
            // Convert point to object space
            var objectPoint = InverseTransform * point;

            var objectNormal = objectPoint - Point.Zero;
            var worldNormal = Matrix4x4.Transpose(InverseTransform) * objectNormal;
            worldNormal = new Vector(worldNormal.X, worldNormal.Y, worldNormal.Z);
            
            return Vector.Normalize(worldNormal);
        }
    }
}