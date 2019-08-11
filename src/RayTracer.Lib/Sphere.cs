namespace RayTracer.Lib
{
    public class Sphere : BaseObject
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
            var transformInverse = Matrix4x4.Inverse(Transform);
            
            // Convert point to object space
            var objectPoint = transformInverse * point;

            var objectNormal = objectPoint - new Point(0, 0, 0);
            var worldNormal = Matrix4x4.Transpose(transformInverse) * objectNormal;
            worldNormal = new Vector(worldNormal.X, worldNormal.Y, worldNormal.Z);
            
            return Vector.Normalize(worldNormal);
        }
    }
}