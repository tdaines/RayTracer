namespace RayTracer.Lib
{
    public abstract class Shape
    {
        public Matrix4x4 Transform { get; }
        public Matrix4x4 InverseTransform { get; }
        private Matrix4x4 TransposedInverseTransform { get; }
        public Material Material { get; }

        protected Shape() : this(Matrix4x4.Identity(), new Material())
        {
        }

        protected Shape(Matrix4x4 transform) : this(transform, new Material())
        {
        }

        protected Shape(Material material) : this(Matrix4x4.Identity(), material)
        {
        }

        protected Shape(Matrix4x4 transform, Material material)
        {
            Transform = transform;
            InverseTransform = Matrix4x4.Inverse(Transform);
            TransposedInverseTransform = Matrix4x4.Transpose(InverseTransform);
            Material = material;
        }
        
        public Ray TransformRay(Ray ray)
        {
            return new Ray(
                ray.Origin * InverseTransform,
                ray.Direction * InverseTransform);
        }

        public Vector Normal(Point point)
        {
            // Convert point to object space
            var objectPoint = InverseTransform * point;
            var objectNormal = ObjectNormal(objectPoint);
            
            var worldNormal = TransposedInverseTransform * objectNormal;
            worldNormal = new Vector(worldNormal.X, worldNormal.Y, worldNormal.Z);
            
            return Vector.Normalize(worldNormal);
        }

        protected abstract Vector ObjectNormal(Point objectPoint);

        public Intersections Intersect(Ray ray)
        {
            ray = TransformRay(ray);

            return IntersectTransformedRay(ray);
        }
        
        protected abstract Intersections IntersectTransformedRay(Ray ray);
    }
}