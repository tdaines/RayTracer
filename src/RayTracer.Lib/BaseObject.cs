using System.Text;

namespace RayTracer.Lib
{
    public abstract class BaseObject
    {
        public Matrix4x4 Transform { get; }
        public Material Material { get; }

        protected BaseObject() : this(Matrix4x4.Identity(), new Material())
        {
        }

        protected BaseObject(Matrix4x4 transform) : this(transform, new Material())
        {
        }

        protected BaseObject(Material material) : this(Matrix4x4.Identity(), material)
        {
        }

        protected BaseObject(Matrix4x4 transform, Material material)
        {
            Transform = transform;
            Material = material;
        }

        public abstract Vector Normal(Point point);
    }
}