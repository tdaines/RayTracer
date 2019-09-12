using System.Text;

namespace RayTracer.Lib
{
    public abstract class Shape
    {
        public Matrix4x4 Transform { get; }
        public Matrix4x4 InverseTransform { get; }
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
            Material = material;
        }

        public abstract Vector Normal(Point point);
    }
}